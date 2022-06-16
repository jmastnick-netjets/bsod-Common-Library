using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bsod.Common.IO
{
    public struct FileVersionChecker
    {
        private FileVersionInfo _fileVersion;
        /// <summary>
        /// Major file version or numbers before the first period
        /// </summary>
        public int Major { get; set; }
        /// <summary>
        /// Minor file version or numbers after the first period before the second period.
        /// </summary>
        public int Minor { get; set; }
        /// <summary>
        /// Build file version or numbers after the second period before the third.
        /// </summary>
        public int Build { get; set; }
        /// <summary>
        /// Revision file version or numbers after the third period.
        /// </summary>
        public int Revision { get; set; }
        /// <summary>
        /// Creates a new file version object, from given path and returns it.
        /// </summary>
        /// <param name="FullPath">Path to get the file version from.</param>
        /// <returns></returns>
        public static FileVersionChecker GetFileVersion(string FullPath)
        {
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(FullPath);
            FileVersionChecker cFvi = new FileVersionChecker(fvi);
            return cFvi;

        }

        /// <summary>
        /// Creates a new C_FileVersionObject, using the FileVersionInfo object given.
        /// </summary>
        /// <param name="fileVersion">FileVersionObject to create the custom FileVersionObject from.</param>
        private FileVersionChecker(FileVersionInfo fileVersion) : this()
        {
            if (fileVersion == null)
                throw new Exception("Cannot create new file version when FileVersionInfo is null.");
            _fileVersion = fileVersion;
            this.Major = _fileVersion.FileMajorPart;
            this.Minor = _fileVersion.FileMinorPart;
            this.Build = _fileVersion.FileBuildPart;
            this.Revision = _fileVersion.FilePrivatePart;
            
        }
        /// <summary>
        /// Returns the string value of the version.
        /// </summary>
        public override string ToString()
        {
            return _fileVersion.FileVersion;
        }

        /// <summary>
        /// Will compare the file version of this object/file to another object/file, if less than returns true if greater than or equals returns false.
        /// Will compare each version set: Major, Minor, Build, and Revision in sequence and if the sequence in order is less returns true.
        /// </summary>
        /// <param name="FileVersion">Object/file to compare to</param>
        public bool IsLessThan(FileVersionChecker FileVersion)
        {
            if (this.Major < FileVersion.Major)
            {
                return true;
            }
            else if (this.Major == FileVersion.Major)
            {
                if (this.Minor < FileVersion.Minor)
                {
                    return true;
                }
                else if (this.Minor == FileVersion.Minor)
                {
                    if (this.Build < FileVersion.Build)
                    {
                        return true;
                    }
                    else if (this.Build == FileVersion.Build)
                    {
                        if (this.Revision < FileVersion.Revision)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Will compare the file version of the first object/file to the second object/file, if less than returns true if greater than or equals returns false.
        /// Will compare each version set: Major, Minor, Build, and Revision in sequence and if the sequence in order is less returns true.
        /// </summary>
        /// <param name="firstFileVersion">Object/file to compare</param>
        /// <param name="secondFileVersion">Object/file to compare to</param>
        public static bool operator<(FileVersionChecker firstFileVersion, FileVersionChecker secondFileVersion)
        {
            return firstFileVersion.IsLessThan(secondFileVersion);
        }
        /// <summary>
        /// Determines if the specified System.Object is equal to the current System.Object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object</param>
        /// <returns>
        /// true if the specified System.Object is equal to the current System.Object;
        ///     otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// Will compare the file version of this object/file to another object/file, if equal returns true and if not equals returns false.
        /// Will compare each version set: Major, Minor, Build, and Revision in sequence and if the sequence in order is equal returns true.
        /// </summary>
        /// <param name="FileVersion">Object/file to compare to</param>
        public bool Equals(FileVersionChecker FileVersion)
        {
            if (this.Major == FileVersion.Major && this.Minor == FileVersion.Minor &&
                this.Build == FileVersion.Build && this.Revision == FileVersion.Revision)
                return true;
            else
                return false;
        }
        ///// <summary>
        ///// Compares firstFileVersion to a generic object type.
        ///// </summary>
        ///// <param name="firstFileVersion">FileVersion object to compare</param>
        ///// <param name="obj">generic object to compare to</param>
        //public static bool operator ==(FileVersionChecker firstFileVersion, object obj)
        //{
        //    return Equals(firstFileVersion, obj);
        //}
        /// <summary>
        /// Will compare the file version of the first object/file to the second object/file, if equal returns true and if does equals returns false.
        /// Will compare each version set: Major, Minor, Build, and Revision in sequence and if the sequence in order is equal returns true.
        /// </summary>
        /// <param name="firstFileVersion">Object/file to compare</param>
        /// <param name="secondFileVersion">Object/file to compare to</param>
        public static bool operator ==(FileVersionChecker firstFileVersion, FileVersionChecker secondFileVersion)
        {
            return firstFileVersion.Equals(secondFileVersion);
        }
        /// <summary>
        /// Will compare the file version of the first object/file to the second object/file, if less than or equal returns true and if greater than returns false.
        /// Will compare each version set: Major, Minor, Build, and Revision in sequence and if the sequence in order is less than or equal returns true.
        /// </summary>
        /// <param name="firstFileVersion">Object/file to compare</param>
        /// <param name="secondFileVersion">Object/file to compare to</param>
        public static bool operator <=(FileVersionChecker firstFileVersion, FileVersionChecker secondFileVersion)
        {
            return firstFileVersion.IsLessThanOrEquals(secondFileVersion);
        }
        /// <summary>
        /// Will compare the file version of this object/file to another object/file, if less than or equal returns true and if greater than returns false.
        /// Will compare each version set: Major, Minor, Build, and Revision in sequence and if the sequence in order is less than or equal returns true.
        /// </summary>
        /// <param name="FileVersion">Object/file to compare to</param>
        public bool IsLessThanOrEquals(FileVersionChecker FileVersion)
        {
            if (this.Equals(FileVersion))
                return true;
            else if (this.IsLessThan(FileVersion))
                return true;
            else
                return false;
        }
        /// <summary>
        /// Will compare the file version of this object/file to another object/file, if greater than returns true and if greater than returns false.
        /// Will compare each version set: Major, Minor, Build, and Revision in sequence and if the sequence in order is greater than returns true.
        /// </summary>
        /// <param name="FileVersion">Object/file to compare to</param>
        public bool IsGreaterThan(FileVersionChecker FileVersion)
        {
            if (this.Major > FileVersion.Major)
            {
                return true;
            }
            else if (this.Major == FileVersion.Major)
            {
                if (this.Minor > FileVersion.Minor)
                {
                    return true;
                }
                else if (this.Minor == FileVersion.Minor)
                {
                    if (this.Build > FileVersion.Build)
                    {
                        return true;
                    }
                    else if (this.Build == FileVersion.Build)
                    {
                        if (this.Revision > FileVersion.Revision)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;

        }
        /// <summary>
        /// Will compare the file version of the first object/file to the second object/file, if greater than returns true and if greater than returns false.
        /// Will compare each version set: Major, Minor, Build, and Revision in sequence and if the sequence in order is greater than returns true.
        /// </summary>
        /// <param name="firstFileVersion">Object/file to compare</param>
        /// <param name="secondFileVersion">Object/file to compare to</param>
        public static bool operator >(FileVersionChecker firstFileVersion, FileVersionChecker secondFileVersion)
        {
            return firstFileVersion.IsGreaterThan(secondFileVersion);
        }
        /// <summary>
        /// Will compare the file version of this object/file to another object/file, if greater than or equal returns true and if greater than returns false.
        /// Will compare each version set: Major, Minor, Build, and Revision in sequence and if the sequence in order is greater than or equal returns true.
        /// </summary>
        /// <param name="FileVersion">Object/file to compare to</param>
        public bool IsGreaterThanOrEquals(FileVersionChecker FileVersion)
        {
            if (this.Equals(FileVersion))
                return true;
            else if (this.IsGreaterThan(FileVersion))
                return true;
            else
                return false;
        }
        /// <summary>
        /// Will compare the file version of the first object/file to the second object/file, if greater than or equal returns true and if greater than returns false.
        /// Will compare each version set: Major, Minor, Build, and Revision in sequence and if the sequence in order is greater than or equal returns true.
        /// </summary>
        /// <param name="firstFileVersion">Object/file to compare</param>
        /// <param name="secondFileVersion">Object/file to compare to</param>
        public static bool operator >=(FileVersionChecker firstFileVersion, FileVersionChecker secondFileVersion)
        {
            return firstFileVersion.IsGreaterThanOrEquals(secondFileVersion);
        }
        ///// <summary>
        ///// Compares firstFileVersion to a generic object type.
        ///// </summary>
        ///// <param name="firstFileVersion">FileVersion object to compare</param>
        ///// <param name="obj">generic object to compare to</param>
        //public static bool operator !=(FileVersionChecker firstFileVersion, object Obj)
        //{
        //    return Equals(firstFileVersion, Obj);
        //}
        /// <summary>
        /// Will compare the file version of the first object/file to the second object/file, if not equal returns true and if equals returns false.
        /// Will compare each version set: Major, Minor, Build, and Revision in sequence and if the sequence in order is equal returns true.
        /// </summary>
        /// <param name="firstFileVersion">Object/file to compare</param>
        /// <param name="secondFileVersion">Object/file to compare to</param>
        public static bool operator !=(FileVersionChecker firstFileVersion, FileVersionChecker secondFileVersion)
        {
            return firstFileVersion.IsNotEqual(secondFileVersion);
        }
        /// <summary>
        /// Will compare the file version of this object/file to another object/file, if not equal returns true and if equals returns false.
        /// Will compare each version set: Major, Minor, Build, and Revision in sequence and if the sequence in order is not equal returns true.
        /// </summary>
        /// <param name="FileVersion">Object/file to compare to</param>
        public bool IsNotEqual(FileVersionChecker FileVersion)
        {
            return !this.Equals(FileVersion);
        }
    }
}
