using bsod.Common.IO;
using bsod.Common.IO.Naitive;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static bsod.Common.IO.Naitive.ShellLinkExtensions;

namespace bsod.Common.IO.Extensions
{
    public static class IO_Extensions
    {
        /// <summary>
        /// Checkes if File is locked, by attempting to open file. If File open failes with an IOException and the HR for the Exception returns an errorCode 32 or 33 will return true
        /// otherwise it returns false.
        /// </summary>
        /// <param name="fle">File to check if locked.</param>
        public static bool IsFileLocked(this FileInfo fle)
        {
            try
            {
                if (!fle.Exists)
                    throw new Exception($"File does not exist.\r\n\tFile Location {fle.FullName}");
                using (fle.Open(FileMode.Open)) { }
            }
            catch (IOException ex)
            {
                int errorCode = Marshal.GetHRForException(ex) & ((1 << 16) - 1);

                return errorCode == 32 || errorCode == 33;
            }

            return false;
        }
        // http://stackoverflow.com/questions/139010/how-to-resolve-a-lnk-in-c-sharp
        internal const int FILE_SHARE_READ = 1;
        internal const int FILE_SHARE_WRITE = 2;

        internal const int CREATION_DISPOSITION_OPEN_EXISTING = 3;

        internal const int FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;

        /// <summary>
        /// Gets the Target Directory or File of a Symbolic Link 
        /// </summary>
        /// <param name="symlink">Symbolic Link to Check</param>
        /// <returns>String representing the Target of the Symbolic Link</returns>
        public static string GetSymbolicLinkTarget(this FileSystemInfo symlink)
        {
            if (IsSymbolicLink(symlink))
            {
                using (SafeFileHandle fileHandle = NativeMethods.CreateFile(symlink.FullName, 0, 2, System.IntPtr.Zero, CREATION_DISPOSITION_OPEN_EXISTING, FILE_FLAG_BACKUP_SEMANTICS, System.IntPtr.Zero))
                {
                    if (fileHandle.IsInvalid)
                        throw new Win32Exception(Marshal.GetLastWin32Error());

                    StringBuilder path = new StringBuilder(512);
                    int size = NativeMethods.GetFinalPathNameByHandle(fileHandle.DangerousGetHandle(), path, path.Capacity, 0);
                    if (size < 0)
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    // The remarks section of GetFinalPathNameByHandle mentions the return being prefixed with "\\?\"
                    // More information about "\\?\" here -> http://msdn.microsoft.com/en-us/library/aa365247(v=VS.85).aspx
                    if (path[0] == '\\' && path[1] == '\\' && path[2] == '?' && path[3] == '\\')
                        return path.ToString().Substring(4);
                    else
                        return path.ToString();
                }
            }
            else
            {
                throw new Win32Exception(String.Format("File or Directory, {0}, is not a Symbolic Link.", symlink.FullName));
            }
        }
        /// <summary>
        /// Generates new path for file including file name. If File already exists in path given will add (#) to the end of the file 
        /// until it finds a name that doesn't already exist or hits the amount of tries listed in CMWME.Variables.IORenameRetry.
        /// </summary>
        /// <param name="OriginalFile">File to create new path for.</param>
        /// <param name="NewPath">Root directory that new path will be created with.</param>
        public static string GenerateNewPath(this FileInfo OriginalFile, string NewPath)
        {
            if (!OriginalFile.Exists) throw new Exception("Original File must exist, to generate new path.");
            if (String.IsNullOrWhiteSpace(NewPath)) throw new Exception("Cannot generate new path when NewPath is null or blank.");
            if (NewPath.Substring(NewPath.Length - 1, 1) == "\\") NewPath += OriginalFile.Name;
            else if (NewPath.IndexOf(OriginalFile.Extension) <= -1) NewPath += String.Format("\\{0}", OriginalFile.Name);
            if (File.Exists(NewPath))
            {
                return NewPath;
            }
            else
            {
                int retryCnt = 1;
                do
                {
                    string tDir = CommonFile.GetDirectory(NewPath, OriginalFile.Name);
                    string tNme = OriginalFile.GetNameWOExtension();
                    string tPath = String.Format("{0}{1}({2}){3}", tDir, tNme, retryCnt, OriginalFile.Extension);
                    if (!File.Exists(tPath))
                        return tPath;
                } while (retryCnt++ <= CommonFile.IORenameRetry);
                throw new Exception($"Could not find a file name that didn't already exist, after {CommonFile.IORenameRetry} retries.");
            }
        }

        /// <summary>
        /// Returns name of file without extension.
        /// </summary>
        /// <param name="File">File to get name of.</param>
        public static string GetNameWOExtension(this FileInfo File)
        {
            string name = File.Name;
            string ext = File.Extension;
            if (name.LastIndexOf(ext) >= name.Length - ext.Length - 1)
                return name.Remove(name.LastIndexOf(ext));
            else
                return name;
        }
        /// <summary>
        /// Gets Shortcut Target Address
        /// </summary>
        /// <param name="filename">File Name</param>
        /// <returns>Target Address</returns>
        public static string GetShortcutTarget(this FileSystemInfo filename)
        {
            ShellLink link = new ShellLink();
            ((IPersistFile)link).Load(filename.FullName, STGM_READ);
            // TODO: if I can get hold of the hwnd call resolve first. This handles moved and renamed files.  
            // ((IShellLinkW)link).Resolve(hwnd, 0) 
            StringBuilder sb = new StringBuilder(MAX_PATH);
            WIN32_FIND_DATAW data = new WIN32_FIND_DATAW();
            ((IShellLinkW)link).GetPath(sb, sb.Capacity, out data, 0);
            Marshal.FinalReleaseComObject(link);
            link = null;
            return sb.ToString();
        }
        /// <summary>
        /// Creates a short cut at directory and filename given using the target given for the shortcut target.
        /// </summary>
        /// <param name="dir">Directory to create the short cut in.</param>
        /// <param name="FileName">File name to give the short cut.</param>
        /// <param name="Target">Target to give the short cut.</param>
        public static void CreateShortcut(this DirectoryInfo dir, string FileName, string Target)
        {
            ShellLink link = new ShellLink();
            ((IShellLinkW)link).SetPath(Target);
            ((IShellLinkW)link).SetDescription(Target);
            ((IPersistFile)link).Save(String.Format("{0}\\{1}", dir.FullName, FileName.IndexOf(".lnk") < 0 ? String.Format("{0}.lnk", FileName) : FileName), true);
            Marshal.FinalReleaseComObject(link);
            link = null;
        }

        /// <summary>
        /// Determins if the File or Directory is a Symbolic Link
        /// </summary>
        /// <param name="file">File or Directory to Check</param>
        /// <returns>True if File or Directory is a Symbolic Link False if it isn't</returns>
        public static bool IsSymbolicLink(this FileSystemInfo file)
        {
            if (file.Exists)
            {
                return file.Attributes.HasFlag(FileAttributes.ReparsePoint);
            }
            else
            {
                throw new Win32Exception(String.Format("File, {0}, Doesn't Exist.", file.FullName));
            }
        }
    }
}
