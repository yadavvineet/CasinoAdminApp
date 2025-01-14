﻿namespace Casino.AdminPortal.Shared
{
    /// <summary>
    /// The Facade Types
    /// </summary>
    public enum FacadeType
    {
       
        /// <summary>
        /// Notice Facade
        /// </summary>
        [QualifiedTypeName("Casino.AdminPortal.BusinessFacades.dll", "Casino.AdminPortal.BusinessFacades.SampleFacade")]
        SampleFacade = 0,
        [QualifiedTypeName("Casino.AdminPortal.BusinessFacades.dll", "Casino.AdminPortal.BusinessFacades.UserFacade")]
        UserFacade = 1,
    }
}
