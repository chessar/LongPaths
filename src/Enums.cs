// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Chessar
{
    /// <summary>
    /// The result of applying the patch to support long paths.
    /// </summary>
    public enum PatchLongPathsResult
    {
        /// <summary>
        /// Patch successfully applied.
        /// </summary>
        Success = 0,
        /// <summary>
        /// The patch is not applied,
        /// because already been done.
        /// </summary>
        AlreadyPatched
    }
}
