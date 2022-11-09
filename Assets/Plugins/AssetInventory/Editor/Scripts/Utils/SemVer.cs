using System;
using System.Text.RegularExpressions;

namespace AssetInventory
{
    public sealed class SemVer : IComparable
    {
        public readonly int Major;
        public readonly int Minor;
        public readonly string MinorQualifier;
        public readonly int Micro;
        public readonly string MicroQualifier;
        public readonly int Patch;

        public bool IsValid;

        private readonly string _originalVersion;
        private readonly Regex _numbersOnly = new Regex("[0-9]*");

        public SemVer(string version)
        {
            IsValid = true;

            _originalVersion = version;
            if (!string.IsNullOrEmpty(version))
            {
                string[] components = version.Split('.');

                // remove characters in first segment, like "v", "final"...
                components[0] = Regex.Replace(components[0], "[^0-9]", "");

                if (int.TryParse(components[0], out Major))
                {
                    if (components.Length >= 2)
                    {
                        Match match = _numbersOnly.Match(components[1]);
                        if (match.Success)
                        {
                            if (int.TryParse(match.Value, out Minor))
                            {
                                if (match.Length < components[1].Length)
                                {
                                    MinorQualifier = components[1].Substring(match.Length);
                                    if (MinorQualifier.StartsWith("-")) MinorQualifier = MinorQualifier.Substring(1);
                                }
                            }
                            else
                            {
                                MinorQualifier = components[1];
                            }
                        }

                        if (components.Length >= 3)
                        {
                            match = _numbersOnly.Match(components[2]);
                            if (match.Success)
                            {
                                if (int.TryParse(match.Value, out Micro))
                                {
                                    if (match.Length < components[2].Length)
                                    {
                                        MicroQualifier = components[2].Substring(match.Length);
                                        if (MicroQualifier.StartsWith("-")) MicroQualifier = MicroQualifier.Substring(1);
                                    }
                                }
                                else
                                {
                                    MicroQualifier = components[2];
                                }
                            }
                            if (components.Length >= 4) int.TryParse(components[3], out Patch);
                        }
                    }
                }
                else
                {
                    IsValid = false;
                }
            }
        }

        public static bool operator ==(SemVer version1, SemVer version2)
        {
            return version1?._originalVersion == version2?._originalVersion;
        }

        public static bool operator !=(SemVer version1, SemVer version2)
        {
            return version1?._originalVersion != version2?._originalVersion;
        }

        public static bool operator >=(SemVer version1, SemVer version2)
        {
            return version1 == version2 || version1 > version2;
        }

        public static bool operator <=(SemVer version1, SemVer version2)
        {
            return version1 == version2 || version1 < version2;
        }

        public static bool operator >(SemVer version1, SemVer version2)
        {
            if (version1 == null) return false;
            if (version2 == null) return true;

            if (version1.Major > version2.Major) return true;
            if (version1.Major < version2.Major) return false;

            if (version1.Minor > version2.Minor) return true;
            if (version1.Minor < version2.Minor) return false;

            if (version1.Micro > version2.Micro) return true;
            if (version1.Micro < version2.Micro) return false;

            if (version1.MicroQualifier != null || version2.MicroQualifier != null)
            {
                if (version1.MicroQualifier == null) return true;
                if (version2.MicroQualifier == null) return false;

                if (string.CompareOrdinal(version1.MicroQualifier, version2.MicroQualifier) > 0) return true;
                if (string.CompareOrdinal(version1.MicroQualifier, version2.MicroQualifier) < 0) return false;
            }

            return version1.Patch > version2.Patch;
        }

        public static bool operator <(SemVer version1, SemVer version2)
        {
            if (version2 == null) return false;
            if (version1 == null) return true;

            if (version1.Major < version2.Major) return true;
            if (version1.Major > version2.Major) return false;

            if (version1.Minor < version2.Minor) return true;
            if (version1.Minor > version2.Minor) return false;

            if (version1.Micro < version2.Micro) return true;
            if (version1.Micro > version2.Micro) return false;

            if (version1.MicroQualifier != null || version2.MicroQualifier != null)
            {
                if (version1.MicroQualifier == null) return false;
                if (version2.MicroQualifier == null) return true;

                if (string.CompareOrdinal(version1.MicroQualifier, version2.MicroQualifier) < 0) return true;
                if (string.CompareOrdinal(version1.MicroQualifier, version2.MicroQualifier) > 0) return false;
            }

            return version1.Patch < version2.Patch;
        }

        private bool Equals(SemVer other)
        {
            return _originalVersion == other._originalVersion;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((SemVer) obj);
        }

        public override int GetHashCode()
        {
            return _originalVersion != null ? _originalVersion.GetHashCode() : 0;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is SemVer version)) return 1;

            if (version == this) return 0;

            return version > this ? -1 : 1;
        }

        public override string ToString()
        {
            return $"Semantic Version '{_originalVersion}' (Valid: {IsValid})";
        }

        public bool OnlyDiffersInPatch(SemVer other)
        {
            return Major == other.Major && Minor == other.Minor;
        }
    }
}