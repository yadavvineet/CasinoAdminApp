﻿namespace Casino.AdminPortal.EntityDataModel
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using Casino.AdminPortal.Shared;

    using MappingType = Casino.AdminPortal.Shared.MappingType;

    /// <summary>
    /// Represents entity converter,
    /// Author		: Nagarro
    /// </summary>
    public sealed class EntityConverter
    {
        #region Ctor

        /// <summary>
        /// Prevents a default instance of the <see cref="EntityConverter"/> class from being created. 
        /// </summary>
        private EntityConverter()
        {
        }

        #endregion

        #region Conversion Methods

        /// <summary>
        /// Fills the DTO from entity.
        /// </summary>
        /// <param name="fromEntity">From entity.</param>
        /// <param name="toDto">To DTO.</param>
        public static void FillDtoFromEntity(object fromEntity, IDto toDto)
        {
            FillData(toDto, fromEntity, false);
        }

        /// <summary>
        /// Fills the DTO from Complex object.
        /// </summary>
        /// <param name="fromComplex">
        /// The from Complex Object.
        /// </param>
        /// <param name="toDto">
        /// To DTO.
        /// </param>
        public static void FillDtoFromComplexObject(object fromComplex, IDto toDto)
        {
            FillData(toDto, fromComplex, false);
        }

        /// <summary>
        /// Fills the entity from DTO.
        /// </summary>
        /// <param name="fromDto">From DTO.</param>
        /// <param name="toEntity">To entity.</param>
        public static void FillEntityFromDto(IDto fromDto, object toEntity)
        {
            FillData(fromDto, toEntity, true);
        }

        #region Private Helper Methods

        /// <summary>
        /// Fills the data.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="entityFromDto">if private set to <c>true</c> [entity from dto].</param>
        private static void FillData(IDto dto, object entity, bool entityFromDto)
        {
            var dtoType = dto.GetType();
            var entityType = entity.GetType();
            MappingType mappingType;

            if (!VerifyForEntityType(entityType, dtoType, out mappingType))
            {
                throw new EntityConversionException(string.Format(Thread.CurrentThread.CurrentCulture, "Entity type '{0}' must match with type specified in EntityMappingAttribute on type '{1}' !", entityType.ToString(), dtoType.ToString()));
            }

            var properties = dtoType.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                bool skipThisProperty = false;
                object[] customAttributes = property.GetCustomAttributes(typeof(EntityPropertyMappingAttribute), false);
                if (mappingType == MappingType.TotalExplicit && customAttributes.Length == 0)
                {
                    continue;
                }

                foreach (object customAttribute in customAttributes)
                {
                    EntityPropertyMappingAttribute entityPropertyMappingAttribute = (EntityPropertyMappingAttribute)customAttribute;
                    if (entityPropertyMappingAttribute.MappingDirection == MappingDirectionType.None)
                    {
                        skipThisProperty = true;
                        break;
                    }
                }

                if (skipThisProperty)
                {
                    continue;
                }

                var entityPropertyName = GetEntityPropertyName(property, mappingType, entityFromDto);
                if (!string.IsNullOrEmpty(entityPropertyName))
                {
                    var entityProperty = entityType.GetProperty(entityPropertyName);

                    if (entityProperty == null)
                    {
                        throw new EntityConversionException(entityPropertyName, entity);
                    }

                    var sourceProperty = entityFromDto ? property : entityProperty;
                    var destinationProperty = entityFromDto ? entityProperty : property;
                    var sourceObject = entityFromDto ? dto : entity;
                    var destinationObject = entityFromDto ? entity : dto;
                    var sourceValue = sourceProperty.GetValue(sourceObject, null);

                    if (destinationProperty.CanWrite)
                    {
                        if (sourceProperty.PropertyType.IsEnum && destinationProperty.PropertyType == typeof(byte))
                        {
                            sourceValue = (byte)(int)sourceValue;
                        }

                        destinationProperty.SetValue(destinationObject, sourceValue, null);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the name of the entity property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="mappingType">Type of the mapping.</param>
        /// <param name="entityFromDto">if set to <c>true</c> [entity from DTO].</param>
        /// <returns></returns>
        private static string GetEntityPropertyName(PropertyInfo property, MappingType mappingType, bool entityFromDto)
        {
            string entityPropertyName = string.Empty;
            var attribute =
                        (EntityPropertyMappingAttribute)
                        Attribute.GetCustomAttribute(property, typeof(EntityPropertyMappingAttribute));

            bool skipMapping = false;

            if (attribute != null)
            {
                if (entityFromDto)
                {
                    skipMapping = !(attribute.MappingDirection == MappingDirectionType.EntityFromDto || attribute.MappingDirection == MappingDirectionType.Both);
                }
                else
                {
                    skipMapping = !(attribute.MappingDirection == MappingDirectionType.DtoFromEntity || attribute.MappingDirection == MappingDirectionType.Both);
                }
            }

            switch (mappingType)
            {
                case MappingType.TotalExplicit:
                {
                    if (attribute == null)
                    {
                        throw new EntityConversionException(
                            string.Format(
                                Thread.CurrentThread.CurrentCulture, 
                                "Property '{0}' should have EntityPropertyMappingAttribute !"), 
                            entityPropertyName);
                    }

                    entityPropertyName = skipMapping ? string.Empty : attribute.MappedEntityPropertyName;
                    break;
                }
                case MappingType.TotalImplicit:
                {
                    if (attribute != null && skipMapping)
                    {
                        entityPropertyName = string.Empty;
                    }
                    else
                    {
                        entityPropertyName = property.Name;
                    }

                    break;
                }
                case MappingType.Hybrid:
                {
                    if (attribute == null)
                    {
                        entityPropertyName = property.Name;
                    }
                    else if (skipMapping)
                    {
                        entityPropertyName = string.Empty;
                    }
                    else
                        entityPropertyName = attribute.MappedEntityPropertyName;
                    break;
                }
                default:
                {
                    break;
                }
            }

            return entityPropertyName;
        }

        /// <summary>
        /// Verifies the type of for entity.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="dtoType">Type of the DTO.</param>
        /// <param name="mappingType">Type of the mapping.</param>
        /// <returns></returns>
        //private static bool VerifyForEntityType(Type entityType, Type DTOType, out MappingType mappingType)
        //{
        //    var attributes = DTOType.GetCustomAttributes(typeof(EntityMappingAttribute), false);
        //    if (attributes.Count() == 1)
        //    {
        //        var mappingAttribute = (EntityMappingAttribute)attributes[0];
        //        mappingType = mappingAttribute.MappingType;
        //        return true;
        //    }

        //    throw new EntityConversionException("Only one EntityMappingAttribute can be applied on type '{0}' !", DTOType.ToString());
        //}

        private static bool VerifyForEntityType(Type entityType, Type dtoType, out MappingType mappingType)
        {
            var attributes = dtoType.GetCustomAttributes(typeof(EntityMappingAttribute), false);
            if (attributes.Count() == 1)
            {
                var mappingAttribute = (EntityMappingAttribute)attributes[0];
                mappingType = mappingAttribute.MappingType;
                return true;
            }

            throw new EntityConversionException("Only one EntityMappingAttribute can be applied on type '{0}' !", dtoType.ToString());
        }




        #endregion

        #endregion

        #region Singleton Access

        //Todo: Remove singleton access and make the class static

        // Note: ThreadStatic is added over Field here to ensure that each thread 
        // has its unique instance of this class.
        // Since multiple simultaneous calls to this class may come, it is safe
        // to have a seperate copy of this functionality class

        private static volatile EntityConverter _instance;
        private static readonly string _instanceLock = "LOCK";

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static EntityConverter Instance
        {
            get
            {
                // create object if not available
                if (_instance == null)
                {
                    lock (_instanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new EntityConverter();
                        }
                    }
                }

                return _instance;
            }
        }

        #endregion
    }
}
