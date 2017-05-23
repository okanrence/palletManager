using Byaxiom.Logger;
using MyAppTools.Services;
using PalletManagement.Core.Domain;
using PalletManagement.Core.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PalletManagement.Web.Setup
{
    public partial class PalletSetup : System.Web.UI.Page
    {
        private readonly IPalletServices _palletService = null;
        private readonly ICustomerServices _customerService = null;

        public PalletSetup()
        {
            _palletService = new PalletServices();
            _customerService = new CustomerServices();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            LoadCustomers();
            MultiView1.SetActiveView(multipleEntry);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ErrorMessage.Visible = false;
            uplaodInfo.Text = "";
            if (isSingleMode())
                AddSinglePallet();
            else
                AddMultiplePallets();

            // LoadPallets();
        }

        private bool isSingleMode()
        {
            if (rdbSetupType.SelectedIndex == 0)
                return true;
            return false;
        }

        private void AddSinglePallet()
        {
            try
            {
                if (alreadyExists(txtStartSerial.Text))
                {
                    displayMessage("Pallet already exists", false);
                    return;
                }

                var oPallet = new Pallet()
                {
                    FacilityId = int.Parse(ddlPlant.SelectedValue),
                    DateAdded = DateTime.Now,
                    PalletCode = txtStartSerial.Text,
                    StatusId = (int)PALLET_STATUS.Available
                };

                _palletService.Add(oPallet);
                ResetForm();
                displayMessage("Pallet Added Successfully", true);


            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);

            }
        }
        private void AddMultiplePallets()
        {
            try
            {
                var palletList = new List<Pallet>();
                var alreadyAddedPallets = string.Empty;
                foreach (GridViewRow row in gdvPallets.Rows)
                {
                    var pallet = new Pallet()
                    {
                        FacilityId = int.Parse(ddlPlant.SelectedValue),
                        DateAdded = DateTime.Now,
                        PalletCode = row.Cells[1].Text,
                        StatusId = (int)PALLET_STATUS.Available
                    };

                    if (!alreadyExists(pallet.PalletCode))
                    {
                        palletList.Add(pallet);
                    }
                    else
                    {
                        alreadyAddedPallets += $"{pallet.PalletCode}|";
                    }
                }
                if (alreadyAddedPallets.EndsWith("|"))
                    alreadyAddedPallets = alreadyAddedPallets.Remove(alreadyAddedPallets.Length - 1, 1);

                var appendDisplay = string.Empty;

                if (!string.IsNullOrEmpty(alreadyAddedPallets))
                    appendDisplay = $"The following pallets already exists: { alreadyAddedPallets }";

                var no = _palletService.Add(palletList);
                ResetForm();
                displayMessage($"{no} Pallets Uploaded. {appendDisplay}", true);
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);
            }
        }

        //private void AddMulitiplePallets()
        //{
        //    var alreadyAddedPallets = string.Empty;

        //    var startSerial = int.Parse(txtStartSerial.Text);
        //    var endSerial = int.Parse(txtEndSerial.Text);

        //    if (endSerial > startSerial){
        //        displayMessage("End Serial cannot be greated than Start Serial", false);
        //        return;
        //    }
        //    var PalletList = new List<Pallet>();
        //    for (var i = startSerial; i <= endSerial; i++)
        //    {
        //        var oPallet = new Pallet()
        //        {
        //            FacilityId = int.Parse(ddlPlant.SelectedValue),
        //            DateAdded = DateTime.Now,
        //            PalletCode = i.ToString(),
        //            StatusId = (int)PALLET_STATUS.Available
        //        };
        //        if (!alreadyExists(oPallet.PalletCode))
        //        {
        //            PalletList.Add(oPallet);
        //        }
        //        else
        //        {
        //            alreadyAddedPallets += $"{oPallet.PalletCode}|";
        //        }
        //    }
        //    if (alreadyAddedPallets.EndsWith("|"))
        //        alreadyAddedPallets = alreadyAddedPallets.Remove(alreadyAddedPallets.Length - 1, 1);

        //    var appendDisplay = string.Empty;
        //    if (!string.IsNullOrEmpty(alreadyAddedPallets))
        //        appendDisplay = $"The following pallets already exists: { alreadyAddedPallets }";

        //    var no = _palletService.Add(PalletList);
        //    ResetForm();
        //    displayMessage($"{no} Pallets Added Successfully. {appendDisplay}", true);
        //}
        private bool alreadyExists(string palletCode)
        {
            return _palletService.GetList().Any(x => x.PalletCode == palletCode);
        }
        private void UpdatePallet()
        {
            try
            {
                var oPallet = new Pallet()
                {
                    //Address = txtAddress.Text,
                    //PalletName = txtPalletName.Text,
                    //EmailAddress = txtEmailAddress.Text,
                    //PhoneNumber = txtPhoneNumber.Text,
                    //ContactPerson = txtContactPerson.Text,
                    //PalletId = int.Parse(hdfPalletId.Value)
                };
                _palletService.Update(oPallet);
                ResetForm();
                displayMessage("Pallet Updated Successfully", true);

            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);
            }
        }

        private void displayMessage(string message, bool isSuccessMsg)
        {
            ErrorMessage.Visible = true;
            if (isSuccessMsg)
                FailureText.Text = $"{message}";
            else
                FailureText.Text = $"ERROR:{message}";

        }

        private void GetExcel()
        {
            string folderPath = Server.MapPath("~/Files/");

            //Check whether Directory (Folder) exists.
            if (!Directory.Exists(folderPath))
            {
                //If Directory (Folder) does not exists. Create it.
                Directory.CreateDirectory(folderPath);
            }

            //Save the File to the Directory (Folder).
            var filePath = $"{folderPath}{Path.GetFileName(FileUpload1.FileName)}{DateTime.Now.ToString("yyyyMMddhhmmss")}";
            FileUpload1.SaveAs(filePath);

            //Display the success message.
            var listFromExcel = _palletService.ReadPalletsFromExcel(filePath);

            gdvPallets.DataSource = listFromExcel;
            gdvPallets.DataBind();

            uplaodInfo.Text = $"{listFromExcel.Count()} pallets found in excel sheet. Please complete the form on the left and click on the Submit button to upload";

        }

        private void ResetForm()
        {
            txtStartSerial.Text = string.Empty;
            ddlCustomer.SelectedIndex = 0;
            hdfPalletId.Value = string.Empty;
            //btnSubmit.Text = "Submit";
            ErrorMessage.Visible = false;
            uplaodInfo.Text = "";
            //Response.Redirect(Page.Request.RawUrl);
        }

        private void LoadFacilities(int customerId)
        {
            try
            {
                var customer = _customerService.GetList()
                    .Where(x => x.CustomerId == customerId).Include(x => x.Facilities)
                    .FirstOrDefault();
                if (customer != null)
                {
                    var facilities = customer
                        .Facilities
                        .Where(x => x.FacilityType == FACILITY_TYPES.PLANT)
                        .ToList();
                    ddlPlant.DataSource = facilities;
                }
                ddlPlant.DataTextField = "FacilityName";
                ddlPlant.DataValueField = "FacilityId";
                ddlPlant.DataBind();
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);
            }
        }

        private void LoadCustomers()
        {
            try
            {
                ddlCustomer.DataSource = _customerService.GetList().ToList();
                ddlCustomer.DataTextField = "CustomerName";
                ddlCustomer.DataValueField = "CustomerId";
                ddlCustomer.DataBind();
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);
            }
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFacilities(int.Parse(ddlCustomer.SelectedValue));
        }

        protected void rdbSetupType_SelectedIndexChanged(object sender, EventArgs e)
        {
            MultiView1.SetActiveView(isSingleMode() ? singleEntry : multipleEntry);
            txtStartSerial.Text = string.Empty;
            ddlCustomer.SelectedIndex = 0;
            ddlPlant.SelectedIndex = 0;
        }

        protected void btnExtract_Click(object sender, EventArgs e)
        {
            GetExcel();
        }
    }
}