﻿using System;
using System.ComponentModel;

namespace Casino.AdminPortal.Shared
{
    /// <summary>
    /// Defines a contract for base DTO,
    /// Author : Nagarro 
    /// </summary>
    public interface IDto : ICloneable, INotifyPropertyChanged, INotifyPropertyChanging
    {
        /// <summary>
        /// gets or sets the state of the object.
        /// </summary>
        /// <value>The state of the object.</value>        
        ObjectStateType ObjectState { get; set; }

        /// <summary>
        /// gets the unique ID.
        /// </summary>
        /// <value>The unique ID.</value>        
        Guid? UniqueId { get; }

        /// <summary>
        /// gets the type of the DTO.
        /// </summary>
        /// <value>The type of the DTO.</value>        
        DtoType DtoType { get; }

    }
}
