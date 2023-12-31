﻿using KarateClub_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KarateClub.Members.UserControls
{
    public partial class ucMemberCardWithFilter : UserControl
    {
        #region Declare Event
        public class MemberSelectedEventArgs : EventArgs
        {
            public int? MemberID { get; }

            public MemberSelectedEventArgs(int? MemberID)
            {
                this.MemberID = MemberID;
            }
        }

        public event EventHandler<MemberSelectedEventArgs> OnMemberSelected;

        public void RaiseOnMemberSelected(int? MemberID)
        {
            RaiseOnMemberSelected(new MemberSelectedEventArgs(MemberID));
        }

        protected virtual void RaiseOnMemberSelected(MemberSelectedEventArgs e)
        {
            OnMemberSelected?.Invoke(this, e);
        }
        #endregion


        private bool _ShowAddMemberButton = true;
        public bool ShowAddMemberButton
        {
            get => _ShowAddMemberButton;

            set
            {
                _ShowAddMemberButton = value;
                btnAddNewMember.Visible = _ShowAddMemberButton;
            }
        }

        private bool _FilterEnabled = true;
        public bool FilterEnabled
        {
            get => _FilterEnabled;

            set
            {
                _FilterEnabled = value;
                gbFilters.Enabled = _FilterEnabled;
            }
        }

        public int? MemberID => ucMemberCard1.MemberID;
        public clsMember SelectedMemberInfo => ucMemberCard1.SelectedMemberInfo;

        public ucMemberCardWithFilter()
        {
            InitializeComponent();
        }
    
        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is Enter (character code 13)
            if (e.KeyChar == (char)13)
            {
                btnFind.PerformClick();
            }

            // to make sure that the user can enter only numbers
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we don't continue because the form is not valid
                MessageBox.Show("Some fields are not valid!, put the mouse over the red icon(s) to see the Error",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            LoadMemberInfo(int.Parse(txtFilterValue.Text.Trim()));
        }

        private void txtFilterValue_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFilterValue.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFilterValue, "This field is required!");
            }
            else
            {
                errorProvider1.SetError(txtFilterValue, null);
            }
        }

        public void FilterFocus()
        {
            txtFilterValue.Focus();
        }

        private void btnAddNewMember_Click(object sender, EventArgs e)
        {
            frmAddEditMember AddNewMember = new frmAddEditMember();
            AddNewMember.GetMemberID += AddNewMember_MemberIDBack;
            AddNewMember.ShowDialog();
        }

        private void AddNewMember_MemberIDBack(int? MemberID)
        {
            txtFilterValue.Text = MemberID.ToString();
            ucMemberCard1.LoadMemberInfo(MemberID);
        }

        private void ucMemberCardWithFilter_Load(object sender, EventArgs e)
        {
            txtFilterValue.Focus();
        }

        public void LoadMemberInfo(int? MemberID)
        {
            txtFilterValue.Text = MemberID.ToString();
            ucMemberCard1.LoadMemberInfo(MemberID);

            if (OnMemberSelected != null && FilterEnabled)
            {
                // Raise the event with a parameter
                RaiseOnMemberSelected(ucMemberCard1.MemberID);
            }
        }

        public void Clear()
        {
            ucMemberCard1.Reset();
        }

    }
}
