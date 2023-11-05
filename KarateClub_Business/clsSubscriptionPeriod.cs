﻿using KarateClub_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateClub_Business
{
    public class clsSubscriptionPeriod
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int PeriodID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Fees { get; set; }
        public bool Paid { get; set; }
        public int MemberID { get; set; }
        public int PaymentID { get; set; }

        public clsSubscriptionPeriod()
        {
            this.PeriodID = -1;
            this.StartDate = DateTime.Now;
            this.EndDate = DateTime.Now;
            this.Fees = -1M;
            this.Paid = false;
            this.MemberID = -1;
            this.PaymentID = -1;

            Mode = enMode.AddNew;
        }

        private clsSubscriptionPeriod(int PeriodID, DateTime StartDate, DateTime EndDate, decimal Fees,
            bool Paid, int MemberID, int PaymentID)
        {
            this.PeriodID = PeriodID;
            this.StartDate = StartDate;
            this.EndDate = EndDate;
            this.Fees = Fees;
            this.Paid = Paid;
            this.MemberID = MemberID;
            this.PaymentID = PaymentID;

            Mode = enMode.Update;
        }

        private bool _AddNewPeriod()
        {
            this.PeriodID = clsSubscriptionPeriodData.AddNewPeriod(this.StartDate, this.EndDate, this.Fees, this.Paid, this.MemberID, this.PaymentID);

            return (this.PeriodID != -1);
        }

        private bool _UpdatePeriod()
        {
            return clsSubscriptionPeriodData.UpdatePeriod(this.PeriodID, this.StartDate, this.EndDate, this.Fees, this.Paid, this.MemberID, this.PaymentID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPeriod())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdatePeriod();
            }

            return false;
        }

        public static clsSubscriptionPeriod Find(int PeriodID)
        {
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;
            decimal Fees = -1M;
            bool Paid = false;
            int MemberID = -1;
            int PaymentID = -1;

            bool IsFound = clsSubscriptionPeriodData.GetPeriodInfoByID(PeriodID, ref StartDate, ref EndDate, ref Fees, ref Paid, ref MemberID, ref PaymentID);

            if (IsFound)
            {
                return new clsSubscriptionPeriod(PeriodID, StartDate, EndDate, Fees, Paid, MemberID, PaymentID);
            }
            else
            {
                return null;
            }
        }

        public static bool DeletePeriod(int PeriodID)
        {
            return clsSubscriptionPeriodData.DeletePeriod(PeriodID);
        }

        public static bool DoesPeriodExist(int PeriodID)
        {
            return clsSubscriptionPeriodData.DoesPeriodExist(PeriodID);
        }

        public static DataTable GetAllSubscriptionPeriods()
        {
            return clsSubscriptionPeriodData.GetAllSubscriptionPeriods();
        }

    }


}