﻿using System.Configuration;

namespace Casino.AdminPortal.Shared
{
    public static class AppConstants
    {
        public struct ConfigurationKeys
        {
            #region "Properties"
            /// <summary>
            /// Constant representing the assemblies' output bin folder name.
            /// </summary>
            public static readonly string OutputBinFolderName = ConfigurationManager.AppSettings["OutputBinFolderName"];

            #endregion
        }
    }
}
