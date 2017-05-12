using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI.Skins;
using Microsoft.Azure;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Web.Configuration;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace StorjshareDynamite
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


        }

        protected void RadCloudUpload1_FileUploaded(object sender, Telerik.Web.UI.CloudFileUploadedEventArgs args)
        {

            string strOriginalFileName = args.FileInfo.OriginalFileName;
            string strFileName = args.FileInfo.KeyName;
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("Azure"));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer blobContainer = blobClient.GetContainerReference("storjsharedynamite");
                CloudBlockBlob fileBlob = blobContainer.GetBlockBlobReference(strFileName);

                using (StreamReader reader = new StreamReader(fileBlob.OpenRead()))
                {

                    string strLog = reader.ReadToEnd();
                    if (strLog.LastIndexOf("with nodeID") != -1)
                    {
                        string strNodeIDtmp = strLog.Substring(strLog.LastIndexOf("with nodeID") + 12);
                        string strNodeID = strNodeIDtmp.Substring(0, strNodeIDtmp.IndexOf("\""));
                        lblNodeID.Text = strNodeID;


                    }



                    if (strLog.LastIndexOf("delta") != -1)
                    {
                        string strDelta = strLog.Substring(strLog.LastIndexOf("delta") + 7);
                        string strDeltaResult = strDelta.Substring(0, strDelta.IndexOf("ms"));
                        int intDelta = Convert.ToInt32(strDeltaResult);
                        if (intDelta > 500 || intDelta < -500)
                        {

                            lblDelta.ForeColor = System.Drawing.Color.Red;
                            lblDelta.Text = strDeltaResult + " |  Your clock is out of sync.  If on Windows please download the Time Sync Tool at www.timesynctool.com";

                        }
                        else
                        {
                            lblDelta.ForeColor = System.Drawing.Color.Green;
                            lblDelta.Text = strDeltaResult + " | Good";
                        }

                        // int count = new Regex(Regex.Escape("PUBLISH")).Matches(strDeltaResult).Count;
                        int count = Regex.Matches(Regex.Escape(strDelta), "PUBLISH").Count;
                        // int offercount = new Regex(Regex.Escape("OFFER")).Matches(strDeltaResult).Count;
                        int offercount = Regex.Matches(Regex.Escape(strDelta), "OFFER").Count;
                        // int validcount = new Regex(Regex.Escape("received valid")).Matches(strDeltaResult).Count;
                        int validcount = Regex.Matches(Regex.Escape(strDelta), "received valid").Count;

                        if (count > 0)
                        {
                            lblReceiving.ForeColor = System.Drawing.Color.Green;
                            lblReceiving.Text = " Yes! You have received " + count.ToString() + " storage messages. | Good.";
                            if (offercount > 0)
                            {
                                lblOFFERS.ForeColor = System.Drawing.Color.Green;
                                lblOFFERS.Text = "You have sent " + offercount.ToString() + " storage offers.  | Good.";
                            }
                            else
                            {
                                lblOFFERS.Text = "";
                            }
                        }
                        else
                        {
                            lblReceiving.ForeColor = System.Drawing.Color.Goldenrod;
                            if (validcount > 0)
                            {
                                lblReceiving.Text = " Yes.  You are receiving responses. However, no requests for storage (PUBLISH) have been received yet. This is normal if your log is less than 1 hour old.";
                                lblOFFERS.Text = "";
                            }
                            else
                            { 
                                lblReceiving.Text = " No.  If other messages don't show an error, your log may be less than an hour old. Please submit again after 1 hour.";
                                lblOFFERS.Text = "";
                            }
                        }



                    }
                    if (strLog.LastIndexOf("your address is public and traversal strategies are disabled") != -1)
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                        lblStatus.Text = " | Your address is public and traversal strategies are disabled.  Good.";
                    }
                    else
                    {
                        lblStatus.Text = "";
                    }







                }



            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
                return;
            }




        }

    }
}