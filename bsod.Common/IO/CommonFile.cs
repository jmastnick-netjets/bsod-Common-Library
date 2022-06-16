using bsod.Common.Extensions;
using bsod.Common.IO.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bsod.Common.IO
{
    /// <summary>
    /// Return values for CompareFileVersion method. Tells you if the first file was equal, less than or greater than the second.
    /// </summary>
    public enum FileVersionCompare
    {
        /// <summary>
        /// Both files versions were equal
        /// </summary>
        IsEqual = 1,
        /// <summary>
        /// First file's version was less than the second's.
        /// </summary>
        IsLessThan = 2,
        /// <summary>
        /// First file's version was greater than the second's.
        /// </summary>
        IsGreaterThan = 4
    }
    /// <summary>
    /// What file copies or moves do if file exists already.
    /// </summary>
    public enum FileExistsWriteType
    {
        /// <summary>
        /// File will be skipped if already exists.
        /// </summary>
        Skip,
        /// <summary>
        /// File will be renamed if exists up to the count given in CMWME.Variables.IORenameRetry, defaulted to 100.
        /// </summary>
        Rename,
        /// <summary>
        /// File will be overridden if already exists.
        /// </summary>
        Overwrite
    }

    /// <summary>
    /// Extra File Functions
    /// </summary>
    public static class CommonFile
    {
        /// <summary>
        /// Amount of times file writes will attempt to rename a file before quiting.
        /// </summary>
        public static int IORenameRetry = 100;
        /// <summary>
        /// Amount of time in milliseconds the IO methods will wait while copying files, creating directories etc. Default wait 
        /// time is 2000 milliseconds.
        /// </summary>
        public static int IOWaitTime = 2000;
        /// <summary>
        /// Different functions to get or check the File Hash
        /// </summary>
        public static class Hash
        {
            /// <summary>
            /// Get the File's Hash String
            /// </summary>
            /// <param name="FilePath"></param>
            /// <returns></returns>
            public static string GetHashString(string FilePath)
            {
                byte[] hash = GetBytes(FilePath);
                return hash.BytesToHexString();
            }
            /// <summary>
            /// Gets the File's Hash and converts it to a hex string value
            /// </summary>
            /// <param name="FilePath">File Path of file to check</param>
            /// <returns>String value in hex of the file hash</returns>
            private static string GetBytesString(string FilePath)
            {
                byte[] hash = GetBytes(FilePath);
                return hash.BytesToByteString();
            }

            /// <summary>
            /// Gets File's Hash in Bytes
            /// </summary>
            /// <param name="FilePath">File path for file to check</param>
            /// <returns>Byte Array of the file hash</returns>
            public static byte[] GetBytes(string FilePath)
            {
                try
                {
                    SHA256 hashConverter = SHA256.Create();
                    byte[] hash;
                    if (File.Exists(FilePath))
                    {
                        using (FileStream fle = new FileStream(FilePath, FileMode.Open))
                        {
                            fle.Position = 0;
                            hash = hashConverter.ComputeHash(fle);
                        }
                        return hash;
                    }
                    else
                    {
                        throw new Exception(String.Format("File {0} does not exist.", FilePath));
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            /// <summary>
            /// Compares the Hash bytes of a file to another file
            /// </summary>
            /// <param name="File1Hash">Hash bytes of a file</param>
            /// <param name="File2Path">path of a file to compare the hash</param>
            /// <returns>true if both files hash matches</returns>
            public static bool CompareHash(byte[] File1Hash, string File2Path)
            {
                try
                {
                    if (File1Hash == GetBytes(File2Path))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            /// <summary>
            /// Compares the Hash string bytes of a file to another file
            /// </summary>
            /// <param name="File1Hash">Hash string bytes of a file</param>
            /// <param name="File2Path">path of a file to compare the hash</param>
            /// <returns>true if both files hash matches</returns>
            public static bool CompareHash(string File1Hash, string File2Path)
            {
                try
                {
                    if (File1Hash.HexStringToBytes().CheckEquality(GetBytes(File2Path)))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            /// <summary>
            /// Compares the Hash string bytes of a file to the bytes of another file
            /// </summary>
            /// <param name="File1Hash">string hash of a file</param>
            /// <param name="File2Hash">bytes hash of a file</param>
            /// <returns>true if both files hash matches</returns>
            public static bool CompareHash(string File1Hash, byte[] File2Hash)
            {
                try
                {
                    if (File1Hash.ByteStringToBytes() == File2Hash)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            /// <summary>
            /// Checkes File1's hash and compares it to File2's hash
            /// </summary>
            /// <param name="File1Path">Path to File1's hash to check</param>
            /// <param name="File2Path">Path to File2's hash to check</param>
            /// <returns>True if the two file's hash match, false if they don't</returns>
            public static bool CompareFiles(string File1Path, string File2Path)
            {
                try
                {
                    if (GetBytes(File1Path) == GetBytes(File2Path))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            
        }

        /// <summary>
        /// Returns a list of files based on the given parameters.
        /// </summary>
        /// <param name="DirectoryPath">Directory path to get the files from.</param>
        /// <param name="Recursive">Whether or not to get the files from sub folders of the directory given.</param>
        /// <param name="Extensions">Specified file extensions to return.</param>
        public static IEnumerable<FileInfo> GetFiles(string DirectoryPath, bool Recursive, IEnumerable<string> Extensions)
        {
            if (String.IsNullOrWhiteSpace(DirectoryPath))
                throw new Exception("Directory Path cannot be null or blank.");
            if (!Directory.Exists(DirectoryPath))
                throw new Exception($"Unable to return a file list of directory path, {DirectoryPath}, does not exist.");
            if (Extensions != null && Extensions.Count() > 0)
            {
                for (int i = 0; i < Extensions.Count(); i++)
                {
                    string ext = Extensions.ElementAt(i);

                    if (ext.Substring(0, 1) == "*")
                        ext = ext.Substring(1, ext.Length - 1);
                    if (ext.Substring(0, 1) == ".")
                        ext = ext.Substring(1, ext.Length - 1);
                    ext = string.Format("*.{0}", ext);
                    IEnumerable<FileInfo> retFles = _getFiles(DirectoryPath, Recursive, ext);
                    if (retFles != null && retFles.Count() > 0)
                    {
                        for (int f = 0; f < retFles.Count(); f++)
                        {
                            yield return retFles.ElementAt(f);
                        }
                    }
                }
            }
            else
            {
                IEnumerable<FileInfo> retFles = _getFiles(DirectoryPath, Recursive, "*");
                if (retFles != null && retFles.Count() > 0)
                {
                    for (int f = 0; f < retFles.Count(); f++)
                    {
                        yield return retFles.ElementAt(f);
                    }
                }
            }

        }
        /// <summary>
        /// Returns a list of files based on the given parameters.
        /// </summary>
        /// <param name="DirectoryPath">Directory path to get the files from.</param>
        /// <param name="Recursive">Whether or not to get the files from sub folders of the directory given.</param>
        /// <param name="Extension">Specified file extension to return.</param>
        private static IEnumerable<FileInfo> _getFiles(string DirectoryPath, bool Recursive, string Extension)
        {
            SearchOption SearchOpt = Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            DirectoryInfo dir = new DirectoryInfo(DirectoryPath);
            return dir.GetFiles(Extension, SearchOpt);
        }
        /// <summary>
        /// Returns a list of files based on the given parameters.
        /// </summary>
        /// <param name="DirectoryPath">Directory path to get the files from.</param>
        /// <param name="Recursive">Whether or not to get the files from sub folders of the directory given.</param>
        /// <param name="Extension">Specified file extensions to return.</param>
        public static IEnumerable<FileInfo> GetFiles(string DirectoryPath, bool Recursive, string Extension)
        {
            return GetFiles(DirectoryPath, Recursive, new string[] { Extension });
        }
        /// <summary>
        /// Returns a list of files based on the given parameters.
        /// </summary>
        /// <param name="DirectoryPath">Directory path to get the files from.</param>
        /// <param name="Recursive">Whether or not to get the files from sub folders of the directory given.</param>
        public static IEnumerable<FileInfo> GetFiles(string DirectoryPath, bool Recursive)
        {
            return GetFiles(DirectoryPath, Recursive, new string[] { });
        }
        /// <summary>
        /// Compares the versions of two files, if they are equal will return FileVersionCompare.IsEqual enum, if the
        /// FirstFilePath is less than the SecondFilePath it will return FileVersionCompare.IsLessThan enum, if the
        /// FirstFilePath is greater than the SecondFilePath it will return FileVersionCompare.IsGreaterThan. If it has
        /// issues with the compare it will throw an Exception with an error code of 100xxx. 
        /// </summary>
        /// <param name="FirstFilePath">File/object to compare</param>
        /// <param name="SecondFilePath">File/object to compare to</param>
        public static FileVersionCompare CompareFileVersions(FileInfo FirstFilePath, string SecondFilePath)
        {
            return CompareFileVersions(FirstFilePath.FullName, SecondFilePath);
        }
        /// <summary>
        /// Compares the versions of two files, if they are equal will return FileVersionCompare.IsEqual enum, if the
        /// FirstFilePath is less than the SecondFilePath it will return FileVersionCompare.IsLessThan enum, if the
        /// FirstFilePath is greater than the SecondFilePath it will return FileVersionCompare.IsGreaterThan. If it has
        /// issues with the compare it will throw an Exception with an error code of 100xxx. 
        /// </summary>
        /// <param name="FirstFilePath">File/object to compare</param>
        /// <param name="SecondFilePath">File/object to compare to</param>
        public static FileVersionCompare CompareFileVersions(FileInfo FirstFilePath, FileInfo SecondFilePath)
        {
            return CompareFileVersions(FirstFilePath.FullName, SecondFilePath.FullName);
        }
        /// <summary>
        /// Compares the versions of two files, if they are equal will return FileVersionCompare.IsEqual enum, if the
        /// FirstFilePath is less than the SecondFilePath it will return FileVersionCompare.IsLessThan enum, if the
        /// FirstFilePath is greater than the SecondFilePath it will return FileVersionCompare.IsGreaterThan. If it has
        /// issues with the compare it will throw an Exception with an error code of 100xxx. 
        /// </summary>
        /// <param name="FirstFilePath">File/object to compare</param>
        /// <param name="SecondFilePath">File/object to compare to</param>
        public static FileVersionCompare CompareFileVersions(string FirstFilePath, FileInfo SecondFilePath)
        {
            return CompareFileVersions(FirstFilePath, SecondFilePath.FullName);
        }
        /// <summary>
        /// Compares the versions of two files, if they are equal will return FileVersionCompare.IsEqual enum, if the
        /// FirstFilePath is less than the SecondFilePath it will return FileVersionCompare.IsLessThan enum, if the
        /// FirstFilePath is greater than the SecondFilePath it will return FileVersionCompare.IsGreaterThan. If it has
        /// issues with the compare it will throw an Exception with an error code of 100xxx. 
        /// </summary>
        /// <param name="FirstFilePath">File/object to compare</param>
        /// <param name="SecondFilePath">File/object to compare to</param>
        public static FileVersionCompare CompareFileVersions(string FirstFilePath, string SecondFilePath)
        {
            if (!File.Exists(FirstFilePath) && !File.Exists(SecondFilePath))
                throw new Exception($"Cannot compare file versions when both files don't exist. First file path {FirstFilePath} and second file path {SecondFilePath}.");
            if (!File.Exists(FirstFilePath))
                throw new Exception($"Cannot compare file versions when first file doesn't exist, path {FirstFilePath}");
            if (!File.Exists(SecondFilePath))
                throw new Exception($"Cannot compare file versions when second file doesn't exist, path {SecondFilePath}");
            FileVersionChecker fVers = FileVersionChecker.GetFileVersion(FirstFilePath);
            if (fVers == null) throw new Exception($"Failed to get FileVersionInfo of first file no error, path {FirstFilePath}");
            FileVersionChecker sVers = FileVersionChecker.GetFileVersion(SecondFilePath);
            if (sVers == null) throw new Exception($"Failed to get FileVersionInfo of second file no error, path {SecondFilePath}");
            if (fVers.Equals(sVers))
                return FileVersionCompare.IsEqual;
            else if (fVers.IsLessThan(sVers))
                return FileVersionCompare.IsLessThan;
            else if (fVers.IsGreaterThan(sVers))
                return FileVersionCompare.IsGreaterThan;
            else
                throw new Exception("Failed version compare Equals, IsLessThan, and IsGreaterThan all failed.");
        }
        /// <summary>
        /// Copies the FileToCopy to the CopyToPath and verifies the Hash is the same after copy then deletes the original file, will retry if fails 
        /// based on the amount given in the RetryCount. Based on the value of WriteType will dictate whether or not the file will be overwritten, 
        /// renamed or skipped if file already exists. If 
        /// File is true you must provide an EncryptKey and the file will be Encrypted before 
        /// copy based on that key, which will be needed to decrypt it. 
        /// </summary>
        /// <param name="FileToMove">File to copy.</param>
        /// <param name="MoveToPath">Path to copy the file to.</param>
        /// <param name="WriteType">This value will determine whether or not the file will be Overwritten, renamed, or skipped if it already exists in the destination.</param>
        /// <param name="RetryCount">Amount of times to retry the copy if fails.</param>
        /// <param name="EncryptFile">Whether or not the file will be encrypted before copy.</param>
        /// <param name="EncryptKey">Key to encrypt file with.</param>
        public static void MoveWithCheck(FileInfo FileToMove, string MoveToPath, FileExistsWriteType WriteType = FileExistsWriteType.Rename,
                                            int RetryCount = 3)//, bool EncryptFile = false, byte[] EncryptKey = null)
        {
            CopyWithCheck(FileToMove, MoveToPath, WriteType, RetryCount);//, EncryptFile, EncryptKey);
            DeleteFile(FileToMove, RetryCount);
        }
        /// <summary>
        /// Copies the FileToCopy to the CopyToPath then deletes the original file, will retry if fails 
        /// based on the amount given in the RetryCount. Based on the value of WriteType will dictate whether or not the file will be overwritten, 
        /// renamed or skipped if file already exists. If EncryptFile is true you must provide an EncryptKey and the file will be Encrypted before 
        /// copy based on that key, which will be needed to decrypt it. 
        /// </summary>
        /// <param name="FileToMove">File to copy.</param>
        /// <param name="MoveToPath">Path to copy the file to.</param>
        /// <param name="WriteType">This value will determine whether or not the file will be Overwritten, renamed, or skipped if it already exists in the destination.</param>
        /// <param name="RetryCount">Amount of times to retry the copy if fails.</param>
        /// <param name="EncryptFile">Whether or not the file will be encrypted before copy.</param>
        /// <param name="EncryptKey">Key to encrypt file with.</param>
        public static void MoveFile(FileInfo FileToMove, string MoveToPath, FileExistsWriteType WriteType = FileExistsWriteType.Rename,
                                        int RetryCount = 3)//, bool EncryptFile = false, byte[] EncryptKey = null)
        {
            CopyFile(FileToMove, MoveToPath, WriteType, RetryCount);//, EncryptFile, EncryptKey);
            DeleteFile(FileToMove, RetryCount);
        }
        /// <summary>
        /// Deletes the file, if fails will retry the amount of times given in RetryCount.
        /// </summary>
        /// <param name="FileToDelete">File deleted.</param>
        /// <param name="RetryCount">Amount of times to attempt.</param>
        public static void DeleteFile(FileInfo FileToDelete, int RetryCount = 3)
        {
            try
            {
                int cRetry = 0; bool deleted = false; Exception fEx = null;
                do
                {
                    try
                    {
                        if (cRetry == 0) Thread.Sleep(IOWaitTime);
                        FileToDelete.Delete();
                        if (FileToDelete.Exists)
                            throw new Exception("Failed to delete file no error.");
                        deleted = true;
                    }
                    catch (Exception ex)
                    {
                        fEx = ex;
                    }
                } while (cRetry++ < RetryCount && !deleted);
                if (fEx != null) throw fEx;
                else if (!deleted && cRetry >= RetryCount) throw new Exception($"Failed to delete original file after copy hit retry count of {RetryCount}.");
                else if (!deleted) throw new Exception("Failed to delete original file after copy, no error message.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete original file, see inner exception for more details.\r\nFileToMove {FileToDelete}", ex);
            }
        }

        /// <summary>
        /// Copies the FileToCopy to the CopyToPath and verifies the Hash is the same after copy, will retry if fails based on the amount given in the 
        /// RetryCount. Based on the value of WriteType will dictate whether or not the file will be overwritten, renamed or skipped if file already 
        /// exists. If EncryptFile is true you must provide an EncryptKey and the file will be Encrypted before copy based on that key, which will 
        /// be needed to decrypt it. 
        /// </summary>
        /// <param name="FileToCopy">File to copy.</param>
        /// <param name="CopyToPath">Path to copy the file to.</param>
        /// <param name="WriteType">This value will determine whether or not the file will be Overwritten, renamed, or skipped if it already exists in the destination.</param>
        /// <param name="RetryCount">Amount of times to retry the copy if fails.</param>
        /// <param name="EncryptFile">Whether or not the file will be encrypted before copy.</param>
        /// <param name="EncryptKey">Key to encrypt file with.</param>
        public static void CopyWithCheck(FileInfo FileToCopy, string CopyToPath, FileExistsWriteType WriteType = FileExistsWriteType.Rename,
                                            int RetryCount = 3)//, bool EncryptFile = false, byte[] EncryptKey = null)
        {
            try
            {
                int refCnt = 0; bool copied = false; Exception fEx = null;
                do
                {
                    copyFile(ref FileToCopy, ref CopyToPath, WriteType, RetryCount);//, EncryptFile, EncryptKey, ref refCnt);
                    try
                    {
                        copied = Hash.CompareFiles(FileToCopy.FullName, CopyToPath);
                    }
                    catch (Exception ex)
                    {
                        fEx = ex;
                    }
                } while (refCnt++ < RetryCount && !copied);
                if (fEx != null) throw fEx;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to copy file or check file hash, see inner exception for details.\r\nFileToCopy: {FileToCopy.FullName}\r\nCopyToPath: {CopyToPath}", ex);
            }
        }
        /// <summary>
        /// Copies the FileToCopy to the CopyToPath, will retry if fails based on the amount given in the RetryCount. Based on the value
        /// of WriteType will dictate whether or not the file will be overwritten, renamed or skipped if file already exists. If EncryptFile is true
        /// you must provide an EncryptKey and the file will be Encrypted before copy based on that key, which will be needed to decrypt it. 
        /// </summary>
        /// <param name="FileToCopy">File to copy.</param>
        /// <param name="CopyToPath">Path to copy the file to.</param>
        /// <param name="WriteType">This value will determine whether or not the file will be Overwritten, renamed, or skipped if it already exists in the destination.</param>
        /// <param name="RetryCount">Amount of times to retry the copy if fails.</param>
        /// <param name="EncryptFile">Whether or not the file will be encrypted before copy.</param>
        /// <param name="EncryptKey">Key to encrypt file with.</param>
        public static void CopyFile(FileInfo FileToCopy, string CopyToPath, FileExistsWriteType WriteType = FileExistsWriteType.Rename,
                                        int RetryCount = 3)//, bool EncryptFile = false, byte[] EncryptKey = null)
        {
            //int refCnt = 0;
            copyFile(ref FileToCopy, ref CopyToPath, WriteType, RetryCount);//, EncryptFile, EncryptKey, ref refCnt);
        }
        /// <summary>
        /// Copies the FileToCopy to the CopyToPath, will retry if fails based on the amount given in the RetryCount. Based on the value
        /// of WriteType will dictate whether or not the file will be overwritten, renamed or skipped if file already exists. If EncryptFile is true
        /// you must provide an EncryptKey and the file will be Encrypted before copy based on that key, which will be needed to decrypt it. 
        /// The value in rCnt is the current retry count attempted, and will be used to keep it from retying more than the retry count.
        /// </summary>
        /// <param name="FileToCopy">File to copy.</param>
        /// <param name="CopyToPath">Path to copy the file to.</param>
        /// <param name="WriteType">This value will determine whether or not the file will be Overwritten, renamed, or skipped if it already exists in the destination.</param>
        /// <param name="RetryCount">Amount of times to retry the copy if fails.</param>
        /// <param name="EncryptFile">Whether or not the file will be encrypted before copy.</param>
        /// <param name="EncryptKey">Key to encrypt file with.</param>
        /// <param name="rCnt">Amount of retries that have already been attempted.</param>
        private static void copyFile(ref FileInfo FileToCopy, ref string CopyToPath, FileExistsWriteType WriteType,
                                        int RetryCount)//, bool EncryptFile, byte[] EncryptKey, ref int rCnt)
        {
            try
            {
                if (FileToCopy == null) 
                    throw new Exception("FileToCopy is null.");

                if (string.IsNullOrWhiteSpace(CopyToPath)) 
                    throw new Exception("Cannot copy file to blank CopyToPath.");
                //if (EncryptFile && (EncryptKey == null || EncryptKey == new byte[0])) 
                //    throw new Exception("Cannot encrypt file with blank EncryptKey");
                if (CopyToPath.Substring(CopyToPath.Length - 1, 1) == "\\")
                    CopyToPath += FileToCopy.Name;
                else if (CopyToPath.IndexOf(FileToCopy.Extension) <= -1) 
                    CopyToPath += String.Format("\\{0}", FileToCopy.Name);
                if (!FileToCopy.Exists) 
                    throw new Exception("Cannot copy file that does not exist.");
                //New file already exists skip copy.
                if (WriteType == FileExistsWriteType.Skip && File.Exists(CopyToPath)) return;
                CreateDirectory(CopyToPath, FileToCopy.Name, RetryCount);
                //if (EncryptFile)
                //    FileToCopy.Encrypt(EncryptKey, RetryCount);
                int cRetry = 0; Exception fEx = null; bool copied = false; bool Overwrite = false;
                if (WriteType == FileExistsWriteType.Overwrite) Overwrite = true;
                else if (WriteType == FileExistsWriteType.Rename) CopyToPath = GenerateNewPath(FileToCopy, CopyToPath);
                do
                {
                    if (cRetry > 0) Thread.Sleep(IOWaitTime);
                    try
                    {

                        FileToCopy.CopyTo(CopyToPath, Overwrite);
                        if (!File.Exists(CopyToPath))
                            throw new Exception("Failed to copy file to path no error.");
                        else
                            copied = true;

                    }
                    catch (Exception ex)
                    {
                        fEx = ex;
                    }

                } while (cRetry++ < RetryCount && !copied);
                if (fEx != null) 
                    throw fEx;
                else if (!copied && cRetry >= RetryCount) 
                    throw new Exception($"Failed to copy the file, hit RetryCount of {RetryCount}.");
                else if (!copied) 
                    throw new Exception("Failed to copy the file, no error thrown, failure in code.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to copy file see inner exception for more details.\r\nFileToCopy: {FileToCopy.FullName}\r\nCopyToPath: {CopyToPath}", ex);
            }

        }
        /// <summary>
        /// Generates a new path for the file based on the NewPath given. Verifies name of file is at the end of the file, and if there's already
        /// another file with that name renames the file till it finds a name that isn't already taken. It attempts to rename the file the amount 
        /// given in the CMWME.Variable.IORenameRetry property, defaulted to 2000 milliseconds.
        /// </summary>
        /// <param name="OriginalFile">Original file that the NewPath is for.</param>
        /// <param name="NewPath">New path for Original file.</param>
        public static string GenerateNewPath(string OriginalFile, string NewPath)
        {
            if (!File.Exists(OriginalFile)) 
                throw new Exception("Original File path must exist, to generate new path.");
            return GenerateNewPath(new FileInfo(OriginalFile), NewPath);
        }
        /// <summary>
        /// Generates a new path for the file based on the NewPath given. Verifies name of file is at the end of the file, and if there's already
        /// another file with that name renames the file till it finds a name that isn't already taken. It attempts to rename the file the amount 
        /// given in the CMWME.Variable.IORenameRetry property, defaulted to 2000 milliseconds.
        /// </summary>
        /// <param name="OriginalFile">Original file that the NewPath is for.</param>
        /// <param name="NewPath">New path for Original file.</param>
        public static string GenerateNewPath(FileInfo OriginalFile, string NewPath)
        {
            return OriginalFile.GenerateNewPath(NewPath);
        }
        /// <summary>
        /// Creates a directory using CreatePath given. If FileName is not null will check to see if
        /// the FileName is at the end of the path, if so it will remove before attempting to create. Prior to creating the 
        /// CreatePath given, if CreatePath already exists returns without attempting. If Exception is thrown during 
        /// Create or directory doesn't exist after creation will retry based on the amount of RetryCount given and 
        /// will wait before attempting again the amount dictated in the property CMWME.Variables.IOWaitTime, defaulted to 
        /// 2000 milliseconds.
        /// </summary>
        /// <param name="CreatePath">Directory path to create.</param>
        /// <param name="FileName">File name to remove from the end of the file path.</param>
        /// <param name="RetryCount">Amount of times to retry to create the directory.</param>
        public static void CreateDirectory(string CreatePath, string FileName = null, int RetryCount = 3)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(CreatePath)) throw new Exception("CreatePath is null.");
                if (!String.IsNullOrWhiteSpace(FileName))
                    CreatePath = GetDirectory(CreatePath, FileName);
                int cRetry = 0; Exception fEx = null; bool created = false;
                if (Directory.Exists(CreatePath)) return;
                do
                {
                    if (cRetry > 0) 
                        Thread.Sleep(IOWaitTime);
                    try
                    {
                        Directory.CreateDirectory(CreatePath);
                        if (!Directory.Exists(CreatePath))
                            throw new Exception("Failed to create directory no error.");
                        else
                            created = true;
                    }
                    catch (Exception ex)
                    {
                        fEx = ex;
                    }
                } while (cRetry++ < RetryCount && !created);
                if (fEx != null) throw fEx;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create directory, see inner exception for more details.\r\nCreatePath: {CreatePath}\r\nFileName: {Functions.IsNull(FileName, "NULL")}", ex);
            }

        }
        /// <summary>
        /// Returns the FilePath with the FileName removed, if FileName doesn't exist returns the FilePath.
        /// </summary>
        /// <param name="FilePath">FilePath to remove the FileName from.</param>
        /// <param name="FileName">FileName to remove from the FilePath.</param>
        public static string GetDirectory(string FilePath, string FileName)
        {
            if (FilePath.LastIndexOf(FileName) >= FilePath.Length - FileName.Length - 1)
                return FilePath.Remove(FilePath.LastIndexOf(FileName));
            else
                return FilePath;
        }
    }
}
