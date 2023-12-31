﻿using KarateClub_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KarateClub.SubscriptionPeriods.UserControls
{
    public partial class ucMemberSubscriptionPeriods : UserControl
    {

        private DataTable _dtAllSubscriptionPeriodForMember;

        private int? _MemberID = null;

        public ucMemberSubscriptionPeriods()
        {
            InitializeComponent();
        }

        private void _RefreshSubscriptionPeriodsList()
        {
            _dtAllSubscriptionPeriodForMember = clsSubscriptionPeriod.GetAllPeriodsForMember(_MemberID);
            dgvSubscriptionPeriodsList.DataSource = _dtAllSubscriptionPeriodForMember;
            lblNumberOfRecords.Text = dgvSubscriptionPeriodsList.Rows.Count.ToString();

            if (dgvSubscriptionPeriodsList.Rows.Count > 0)
            {
                dgvSubscriptionPeriodsList.Columns[0].HeaderText = "Period ID";
                dgvSubscriptionPeriodsList.Columns[0].Width = 100;

                dgvSubscriptionPeriodsList.Columns[1].HeaderText = "Member Name";
                dgvSubscriptionPeriodsList.Columns[1].Width = 150;

                dgvSubscriptionPeriodsList.Columns[2].HeaderText = "Fees";
                dgvSubscriptionPeriodsList.Columns[2].Width = 110;

                dgvSubscriptionPeriodsList.Columns[3].HeaderText = "Is Paid";
                dgvSubscriptionPeriodsList.Columns[3].Width = 100;

                dgvSubscriptionPeriodsList.Columns[4].HeaderText = "Start Date";
                dgvSubscriptionPeriodsList.Columns[4].Width = 106;

                dgvSubscriptionPeriodsList.Columns[5].HeaderText = "End Date";
                dgvSubscriptionPeriodsList.Columns[5].Width = 105;

                dgvSubscriptionPeriodsList.Columns[6].HeaderText = "Subscription Days";
                dgvSubscriptionPeriodsList.Columns[6].Width = 160;

                dgvSubscriptionPeriodsList.Columns[7].HeaderText = "Payment ID";
                dgvSubscriptionPeriodsList.Columns[7].Width = 115;

                dgvSubscriptionPeriodsList.Columns[8].HeaderText = "Is Active";
                dgvSubscriptionPeriodsList.Columns[8].Width = 100;
            }



        }

        private int _GetSubscriptionPeriodIDFromDGV()
        {
            return (int)dgvSubscriptionPeriodsList.CurrentRow.Cells["PeriodID"].Value;
        }

        public void LoadSubscriptionPeriodsInfo(int? MemberID)
        {
            this._MemberID = MemberID;
            _RefreshSubscriptionPeriodsList();
        }

        public void Clear()
        {
            _dtAllSubscriptionPeriodForMember.Clear();
        }

        private void ShowPeriodDetailstoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmShowSubscriptionPeriodDetails ShowSubscriptionPeriodDetails =
                new frmShowSubscriptionPeriodDetails(_GetSubscriptionPeriodIDFromDGV());

            ShowSubscriptionPeriodDetails.ShowDialog();
        }

        private void payToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to pay for this period?", "Confirm", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {

                clsSubscriptionPeriod Period = clsSubscriptionPeriod.
                    Find((int)dgvSubscriptionPeriodsList.CurrentRow.Cells["PeriodID"].Value);

                if (Period == null)
                {
                    MessageBox.Show("Pay Failed", "Failed",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                int? PaymentID = Period.Pay((decimal)dgvSubscriptionPeriodsList.CurrentRow.Cells["Fees"].Value);

                if (PaymentID.HasValue)
                {
                    Period.PaymentID = PaymentID;
                    Period.IsPaid = (PaymentID.HasValue);

                    if (Period.Save())
                    {
                        MessageBox.Show("Pay Done Successfully", "Deleted",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                        _RefreshSubscriptionPeriodsList();
                    }
                }
                else
                {
                    MessageBox.Show("Deleted Failed", "Failed",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cmsEditProfile_Opening(object sender, CancelEventArgs e)
        {
            payToolStripMenuItem.Enabled = !(bool)dgvSubscriptionPeriodsList.CurrentRow.Cells["IsPaid"].Value;
        }

        private void dgvSubscriptionPeriodsList_DoubleClick(object sender, EventArgs e)
        {
            frmShowSubscriptionPeriodDetails ShowSubscriptionPeriodDetails =
                new frmShowSubscriptionPeriodDetails(_GetSubscriptionPeriodIDFromDGV());

            ShowSubscriptionPeriodDetails.ShowDialog();
        }
    }
}
