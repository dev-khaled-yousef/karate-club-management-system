﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub_DataAccess;

namespace KarateClub_Business
{
    public class clsPerson
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public enum enGender { Male = 0, Female = 1 };

        public int? PersonID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public enGender Gender { get; set; }
        public string ImagePath { get; set; }
        public string GenderName => _GetGenderName(this.Gender);

        public clsPerson()
        {
            this.PersonID = null;
            this.Name = string.Empty;
            this.Address = null;
            this.Phone = string.Empty;
            this.Email = null;
            this.DateOfBirth = DateTime.Now;
            this.Gender = enGender.Male;
            this.ImagePath = null;

            Mode = enMode.AddNew;
        }

        private clsPerson(int? PersonID, string Name, string Address,
            string Phone, string Email, DateTime DateOfBirth,
            enGender Gender, string ImagePath)
        {
            this.PersonID = PersonID;
            this.Name = Name;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.DateOfBirth = DateOfBirth;
            this.Gender = Gender;
            this.ImagePath = ImagePath;

            Mode = enMode.Update;
        }

        private bool _AddNewPerson()
        {
            this.PersonID = clsPersonData.AddNewPerson(this.Name, this.Address, this.Phone,
                this.Email, this.DateOfBirth, (byte)this.Gender, this.ImagePath);

            return (this.PersonID.HasValue);
        }

        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(this.PersonID, this.Name, this.Address,
                this.Phone, this.Email, this.DateOfBirth, (byte)this.Gender, this.ImagePath);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdatePerson();
            }

            return false;
        }

        public static clsPerson Find(int? PersonID)
        {
            string Name = string.Empty;
            string Address = null;
            string Phone = string.Empty;
            string Email = null;
            DateTime DateOfBirth = DateTime.Now;
            byte Gender = 0;
            string ImagePath = null;

            bool IsFound = clsPersonData.GetPersonInfoByID(PersonID, ref Name, ref Address,
                ref Phone, ref Email, ref DateOfBirth, ref Gender, ref ImagePath);

            if (IsFound)
            {
                return new clsPerson(PersonID, Name, Address, Phone, Email,
                    DateOfBirth, (enGender)Gender, ImagePath);
            }
            else
            {
                return null;
            }
        }

        public static bool DeletePerson(int? PersonID)
        {
            return clsPersonData.DeletePerson(PersonID);
        }

        public virtual bool Delete()
        {
            return DeletePerson(this.PersonID);
        }

        public static bool DoesPersonExist(int? PersonID)
        {
            return clsPersonData.DoesPersonExist(PersonID);
        }

        public static DataTable GetAllPeople()
        {
            return clsPersonData.GetAllPeople();
        }

        private string _GetGenderName(enGender gender)
        {
            switch (gender)
            {
                case enGender.Male:
                    return "Male";

                case enGender.Female:
                    return "Female";

                default:
                    return "Unknown";
            }
        }

    }
}
