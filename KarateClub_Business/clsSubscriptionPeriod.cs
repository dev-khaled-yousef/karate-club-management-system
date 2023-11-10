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

        public enum enIssueReason { FirstTime = 1, Renew = 2 };

        public int PeriodID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Fees { get; set; }
        public bool IsPaid { get; set; }
        public int MemberID { get; set; }
        public int PaymentID { get; set; }
        public enIssueReason IssueReason { get; set; }
        public bool IsActive { get; set; }

        public clsMember MemberInfo { get; set; }
        public clsPayment PaymentInfo { get; set; }
        public string IssueReasonText => _IssueReasonText(this.IssueReason);

        public clsSubscriptionPeriod()
        {
            this.PeriodID = -1;
            this.StartDate = DateTime.Now;
            this.EndDate = DateTime.Now;
            this.Fees = -1M;
            this.IsPaid = false;
            this.MemberID = -1;
            this.PaymentID = -1;
            this.IssueReason = enIssueReason.FirstTime;
            this.IsActive = true;

            this.Mode = enMode.AddNew;
        }

        private clsSubscriptionPeriod(int PeriodID, DateTime StartDate,
            DateTime EndDate, decimal Fees, bool IsPaid, int MemberID,
            int PaymentID, enIssueReason IssueReason, bool IsActive)
        {
            this.PeriodID = PeriodID;
            this.StartDate = StartDate;
            this.EndDate = EndDate;
            this.Fees = Fees;
            this.IsPaid = IsPaid;
            this.MemberID = MemberID;
            this.PaymentID = PaymentID;
            this.IssueReason = IssueReason;
            this.IsActive = IsActive;

            this.MemberInfo = clsMember.Find(MemberID);

            if (PaymentID != -1)
                this.PaymentInfo = clsPayment.Find(PaymentID);

            this.Mode = enMode.Update;
        }

        private bool _AddNewPeriod()
        {
            this.PeriodID = clsSubscriptionPeriodData.AddNewPeriod(this.StartDate, this.EndDate,
                this.Fees, this.IsPaid, this.MemberID, this.PaymentID, (byte)this.IssueReason);

            return (this.PeriodID != -1);
        }

        private bool _UpdatePeriod()
        {
            return clsSubscriptionPeriodData.UpdatePeriod(this.PeriodID, this.StartDate,
                this.EndDate, this.Fees, this.IsPaid, this.MemberID, this.PaymentID,
                (byte)this.IssueReason, this.IsActive);
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
            bool IsPaid = false;
            int MemberID = -1;
            int PaymentID = -1;
            byte IssueReason = 0;
            bool isActive = true;

            bool IsFound = clsSubscriptionPeriodData.GetPeriodInfoByID(PeriodID, ref StartDate,
                ref EndDate, ref Fees, ref IsPaid, ref MemberID, ref PaymentID,
                ref IssueReason, ref isActive);

            if (IsFound)
            {
                return new clsSubscriptionPeriod(PeriodID, StartDate, EndDate, Fees,
                    IsPaid, MemberID, PaymentID, (enIssueReason)IssueReason, isActive);
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

        public static short Count()
        {
            return clsSubscriptionPeriodData.CountSubscriptionPeriods();
        }

        public int Pay(decimal Amount)
        {
            clsPayment Payment = new clsPayment();

            Payment.MemberID = this.MemberID;
            Payment.Amount = Amount;

            if (!Payment.Save())
            {
                this.PaymentID = -1;
                this.IsPaid = false;
                return -1;
            }

            this.PaymentID = Payment.PaymentID;
            this.IsPaid = true;

            this.Save();

            clsMember.SetActivity(this.MemberID, true);

            return Payment.PaymentID;
        }

        public static int GetLastActivePeriodIDForMember(int MemberID)
        {
            return clsSubscriptionPeriodData.GetLastActivePeriodForMember(MemberID);
        }

        public static DataTable GetAllPeriodsForMember(int MemberID)
        {
            return clsSubscriptionPeriodData.GetAllPeriodsForMember(MemberID);
        }

        private string _IssueReasonText(enIssueReason IssueReason)
        {
            switch (IssueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";

                case enIssueReason.Renew:
                    return "Renew";

                default:
                    return "First Time";
            }
        }

    }
}
