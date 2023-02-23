﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[assembly: EdmSchemaAttribute()]
namespace MVCProject.Api.Models
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class MVCProjectEntities : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new MVCProjectEntities object using the connection string found in the 'MVCProjectEntities' section of the application configuration file.
        /// </summary>
        public MVCProjectEntities() : base("name=MVCProjectEntities", "MVCProjectEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new MVCProjectEntities object.
        /// </summary>
        public MVCProjectEntities(string connectionString) : base(connectionString, "MVCProjectEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new MVCProjectEntities object.
        /// </summary>
        public MVCProjectEntities(EntityConnection connection) : base(connection, "MVCProjectEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<ApplicantRegister> ApplicantRegisters
        {
            get
            {
                if ((_ApplicantRegisters == null))
                {
                    _ApplicantRegisters = base.CreateObjectSet<ApplicantRegister>("ApplicantRegisters");
                }
                return _ApplicantRegisters;
            }
        }
        private ObjectSet<ApplicantRegister> _ApplicantRegisters;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Designation> Designations
        {
            get
            {
                if ((_Designations == null))
                {
                    _Designations = base.CreateObjectSet<Designation>("Designations");
                }
                return _Designations;
            }
        }
        private ObjectSet<Designation> _Designations;

        #endregion

        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the ApplicantRegisters EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToApplicantRegisters(ApplicantRegister applicantRegister)
        {
            base.AddObject("ApplicantRegisters", applicantRegister);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Designations EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToDesignations(Designation designation)
        {
            base.AddObject("Designations", designation);
        }

        #endregion

    }

    #endregion

    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="MVCProjectModel", Name="ApplicantRegister")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class ApplicantRegister : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new ApplicantRegister object.
        /// </summary>
        /// <param name="applicantId">Initial value of the ApplicantId property.</param>
        public static ApplicantRegister CreateApplicantRegister(global::System.Int32 applicantId)
        {
            ApplicantRegister applicantRegister = new ApplicantRegister();
            applicantRegister.ApplicantId = applicantId;
            return applicantRegister;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 ApplicantId
        {
            get
            {
                return _ApplicantId;
            }
            set
            {
                if (_ApplicantId != value)
                {
                    OnApplicantIdChanging(value);
                    ReportPropertyChanging("ApplicantId");
                    _ApplicantId = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("ApplicantId");
                    OnApplicantIdChanged();
                }
            }
        }
        private global::System.Int32 _ApplicantId;
        partial void OnApplicantIdChanging(global::System.Int32 value);
        partial void OnApplicantIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                OnNameChanging(value);
                ReportPropertyChanging("Name");
                _Name = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Name");
                OnNameChanged();
            }
        }
        private global::System.String _Name;
        partial void OnNameChanging(global::System.String value);
        partial void OnNameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Email
        {
            get
            {
                return _Email;
            }
            set
            {
                OnEmailChanging(value);
                ReportPropertyChanging("Email");
                _Email = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Email");
                OnEmailChanged();
            }
        }
        private global::System.String _Email;
        partial void OnEmailChanging(global::System.String value);
        partial void OnEmailChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Phone
        {
            get
            {
                return _Phone;
            }
            set
            {
                OnPhoneChanging(value);
                ReportPropertyChanging("Phone");
                _Phone = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Phone");
                OnPhoneChanged();
            }
        }
        private global::System.String _Phone;
        partial void OnPhoneChanging(global::System.String value);
        partial void OnPhoneChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Address
        {
            get
            {
                return _Address;
            }
            set
            {
                OnAddressChanging(value);
                ReportPropertyChanging("Address");
                _Address = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Address");
                OnAddressChanged();
            }
        }
        private global::System.String _Address;
        partial void OnAddressChanging(global::System.String value);
        partial void OnAddressChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String EntryBy
        {
            get
            {
                return _EntryBy;
            }
            set
            {
                OnEntryByChanging(value);
                ReportPropertyChanging("EntryBy");
                _EntryBy = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("EntryBy");
                OnEntryByChanged();
            }
        }
        private global::System.String _EntryBy;
        partial void OnEntryByChanging(global::System.String value);
        partial void OnEntryByChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> EntryDate
        {
            get
            {
                return _EntryDate;
            }
            set
            {
                OnEntryDateChanging(value);
                ReportPropertyChanging("EntryDate");
                _EntryDate = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("EntryDate");
                OnEntryDateChanged();
            }
        }
        private Nullable<global::System.DateTime> _EntryDate;
        partial void OnEntryDateChanging(Nullable<global::System.DateTime> value);
        partial void OnEntryDateChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String UpdatedBy
        {
            get
            {
                return _UpdatedBy;
            }
            set
            {
                OnUpdatedByChanging(value);
                ReportPropertyChanging("UpdatedBy");
                _UpdatedBy = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("UpdatedBy");
                OnUpdatedByChanged();
            }
        }
        private global::System.String _UpdatedBy;
        partial void OnUpdatedByChanging(global::System.String value);
        partial void OnUpdatedByChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> UpdateDate
        {
            get
            {
                return _UpdateDate;
            }
            set
            {
                OnUpdateDateChanging(value);
                ReportPropertyChanging("UpdateDate");
                _UpdateDate = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("UpdateDate");
                OnUpdateDateChanged();
            }
        }
        private Nullable<global::System.DateTime> _UpdateDate;
        partial void OnUpdateDateChanging(Nullable<global::System.DateTime> value);
        partial void OnUpdateDateChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Boolean> IsActive
        {
            get
            {
                return _IsActive;
            }
            set
            {
                OnIsActiveChanging(value);
                ReportPropertyChanging("IsActive");
                _IsActive = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("IsActive");
                OnIsActiveChanged();
            }
        }
        private Nullable<global::System.Boolean> _IsActive;
        partial void OnIsActiveChanging(Nullable<global::System.Boolean> value);
        partial void OnIsActiveChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> ApplicantDate
        {
            get
            {
                return _ApplicantDate;
            }
            set
            {
                OnApplicantDateChanging(value);
                ReportPropertyChanging("ApplicantDate");
                _ApplicantDate = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("ApplicantDate");
                OnApplicantDateChanged();
            }
        }
        private Nullable<global::System.DateTime> _ApplicantDate;
        partial void OnApplicantDateChanging(Nullable<global::System.DateTime> value);
        partial void OnApplicantDateChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> DateOfBirth
        {
            get
            {
                return _DateOfBirth;
            }
            set
            {
                OnDateOfBirthChanging(value);
                ReportPropertyChanging("DateOfBirth");
                _DateOfBirth = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("DateOfBirth");
                OnDateOfBirthChanged();
            }
        }
        private Nullable<global::System.DateTime> _DateOfBirth;
        partial void OnDateOfBirthChanging(Nullable<global::System.DateTime> value);
        partial void OnDateOfBirthChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String CurrentCompany
        {
            get
            {
                return _CurrentCompany;
            }
            set
            {
                OnCurrentCompanyChanging(value);
                ReportPropertyChanging("CurrentCompany");
                _CurrentCompany = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("CurrentCompany");
                OnCurrentCompanyChanged();
            }
        }
        private global::System.String _CurrentCompany;
        partial void OnCurrentCompanyChanging(global::System.String value);
        partial void OnCurrentCompanyChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String CurrentDesignation
        {
            get
            {
                return _CurrentDesignation;
            }
            set
            {
                OnCurrentDesignationChanging(value);
                ReportPropertyChanging("CurrentDesignation");
                _CurrentDesignation = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("CurrentDesignation");
                OnCurrentDesignationChanged();
            }
        }
        private global::System.String _CurrentDesignation;
        partial void OnCurrentDesignationChanging(global::System.String value);
        partial void OnCurrentDesignationChanged();

        #endregion

    
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="MVCProjectModel", Name="Designation")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Designation : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Designation object.
        /// </summary>
        /// <param name="designationId">Initial value of the DesignationId property.</param>
        public static Designation CreateDesignation(global::System.Int32 designationId)
        {
            Designation designation = new Designation();
            designation.DesignationId = designationId;
            return designation;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 DesignationId
        {
            get
            {
                return _DesignationId;
            }
            set
            {
                if (_DesignationId != value)
                {
                    OnDesignationIdChanging(value);
                    ReportPropertyChanging("DesignationId");
                    _DesignationId = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("DesignationId");
                    OnDesignationIdChanged();
                }
            }
        }
        private global::System.Int32 _DesignationId;
        partial void OnDesignationIdChanging(global::System.Int32 value);
        partial void OnDesignationIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String DesignationName
        {
            get
            {
                return _DesignationName;
            }
            set
            {
                OnDesignationNameChanging(value);
                ReportPropertyChanging("DesignationName");
                _DesignationName = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("DesignationName");
                OnDesignationNameChanged();
            }
        }
        private global::System.String _DesignationName;
        partial void OnDesignationNameChanging(global::System.String value);
        partial void OnDesignationNameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Boolean> IsActive
        {
            get
            {
                return _IsActive;
            }
            set
            {
                OnIsActiveChanging(value);
                ReportPropertyChanging("IsActive");
                _IsActive = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("IsActive");
                OnIsActiveChanged();
            }
        }
        private Nullable<global::System.Boolean> _IsActive;
        partial void OnIsActiveChanging(Nullable<global::System.Boolean> value);
        partial void OnIsActiveChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Remarks
        {
            get
            {
                return _Remarks;
            }
            set
            {
                OnRemarksChanging(value);
                ReportPropertyChanging("Remarks");
                _Remarks = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Remarks");
                OnRemarksChanged();
            }
        }
        private global::System.String _Remarks;
        partial void OnRemarksChanging(global::System.String value);
        partial void OnRemarksChanged();

        #endregion

    
    }

    #endregion

    
}
