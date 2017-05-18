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
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Net;
using System.Data;
using System.Text;

namespace StorjshareDynamite
{
    public partial class _Default : StorjBase
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

                using (StreamReader reader = new StreamReader(fileBlob.OpenRead(),Encoding.UTF8))
                {

                    string strLog = reader.ReadToEnd();
                    if (strLog.LastIndexOf("with nodeID") != -1)
                    {
                        string strNodeIDtmp = strLog.Substring(strLog.LastIndexOf("with nodeID") + 12);
                        string strNodeID = strNodeIDtmp.Substring(0, strNodeIDtmp.IndexOf('"'));
                        lblNodeID.Text = strNodeID;

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.storj.io/contacts/" + strNodeID);
                        request.Method = "GET";
                        request.Timeout = 30000;
                        request.ContentType = "application/json";
                        string strUUID = UURAND();
                        using (Stream dataStream = request.GetResponse().GetResponseStream())
                        {

                            StreamReader responseReader = new StreamReader(dataStream);
                            string response2 = responseReader.ReadToEnd();
                            var lstCONTACT = JsonConvert.DeserializeObject<CONTACT>(response2);
                            // Now we want to test if the node is reachable.
  
                            bool boolIsPortOpen = IsPortOpen(lstCONTACT.address, lstCONTACT.port);

                            if (boolIsPortOpen == true)
                            {
                                lblOnline.ForeColor = System.Drawing.Color.Green;
                                lblOnline.Text = "Your node is reachable.";
                            }
                            else
                            {
                                lblOnline.ForeColor = System.Drawing.Color.Red;
                                lblOnline.Text = "Your node is not reachable.  Your ports may be closed or your RPC Address is incorrect. Configure port forwarding, you can find information <a target='_blank' href='https://docs.google.com/document/d/1Q87QzIn5UwskzdEaU1zoo7URrkl6Na7FYKrR5TeWNdw'>in this document</a> if on the GUI, or you can read <a target='_blank' href='https://docs.google.com/document/d/1D3VJAz_mDbeki8wiErw2000dzltisqHFxfRFEdqYE_8/edit'>this document</a> for help with the command line daemon.";

                            }

                            lblLastSeen.Text = lstCONTACT.lastSeen;
                            lblAddress.Text = lstCONTACT.address;
                            lblPort.Text = lstCONTACT.port.ToString();
                            lblLastTimeout.Text = lstCONTACT.lastTimeout;
                            lblProtocol.Text = lstCONTACT.protocol;
                            lblTimeoutRate.Text = lstCONTACT.timeoutRate.ToString();
                            lblUserAgent.Text = lstCONTACT.userAgent;
                            lblResponseTime.Text = lstCONTACT.responseTime;



                        }

                    }


                    if (strLog.LastIndexOf("delta") != -1)
                    {
                        string strDelta = strLog.Substring(strLog.LastIndexOf("delta") + 7);
                        string strDeltaResult = strDelta.Substring(0, strDelta.IndexOf("ms"));
                        int intDelta = Convert.ToInt32(strDeltaResult);
                        if (intDelta > 500 || intDelta < -500)
                        {

                            lblDelta.ForeColor = System.Drawing.Color.Red;
                            lblDelta.Text = strDeltaResult + " |  Your clock is out of sync. If on Windows please download the Time Sync Tool at <a target='_blank' href='http://www.timesynctool.com'>www.timesynctool.com</a>";

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
                    else
                    {
                        lblDelta.ForeColor = System.Drawing.Color.Red;
                        lblDelta.Text = " | Cannot find your delta in the log.  Please stop Storj Share.  Erase your log.  Start Storj Share.  Wait 1 hour and then upload your log again.";
                    }

                    if (strLog.LastIndexOf("System clock is not syncronized with NTP") != -1 || strLog.LastIndexOf("Timeout waiting for NTP response") != -1)
                    {
                        lblNTP.ForeColor = System.Drawing.Color.Red;
                        lblNTP.Text = " | Your clock is not synced with a time server. If on Windows please download the Time Sync Tool at <a target='_blank' href='http://www.timesynctool.com'>www.timesynctool.com</a>";
                    }
                    else
                    {
                        lblNTP.ForeColor = System.Drawing.Color.Green;
                        lblNTP.Text = "";
                    }

                    bool boolSkipTraversal = false;

                    if (strLog.LastIndexOf("your address is public and traversal strategies are disabled") != -1)
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                        lblStatus.Text = "Your address is public and traversal strategies are disabled.  Good.";
                        boolSkipTraversal = true;
                    }
                    if (strLog.LastIndexOf("you are not publicly reachable") != -1 && boolSkipTraversal == false)
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Goldenrod;
                        lblStatus.Text = " It looks like you have not setup port forwarding. Your node may still work, but this is not optimal. Configure port forwarding, you can find information <a target='_blank' href='https://docs.google.com/document/d/1Q87QzIn5UwskzdEaU1zoo7URrkl6Na7FYKrR5TeWNdw'>in this document</a> if on the GUI, or you can read <a target='_blank' href='https://docs.google.com/document/d/1D3VJAz_mDbeki8wiErw2000dzltisqHFxfRFEdqYE_8/edit'>this document</a> for help with the command line daemon.";
                    }
                    if (strLog.LastIndexOf("NAT via UPnP") != -1)
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Goldenrod;
                        lblStatus.Text = "Your node is using UPnP to open traffic in your router.  This works, but is not optimal.  Consider forwarding your ports if possible.";
                    }
                    if (strLog.LastIndexOf("your address is private and traversal strategies are disabled") != -1)
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Goldenrod;
                        lblStatus.Text = "Your node is tunneling.  This works, but is not optimal.  Considering forwarding your ports if possible.";
                    }

                    var lstPUBLISH = JsonConvert.DeserializeObject<List<publish>>(strLog);
                    // var StorjFrame = JsonConvert.DeserializeObject<List<StorjFrame>>(response2);

                    int intCounter1 = 0;
                    int intCounter2 = 0;
                    string strFirstDay = "";
                    string strLastDay = "";
                    int intDaycount = 0;
                    DateTime dtDate = DateTime.Now;
                    DataTable tablePubDates = new DataTable();
                    tablePubDates.Clear();
                    tablePubDates.Columns.Add("Date");
                    tablePubDates.Columns.Add("Count");


                    if (lstPUBLISH.Any())
                    {
                        var items = from item in lstPUBLISH
                                    where item.message.Contains("sending PUBLISH message to")
                                    select item;


                        foreach (var item in items)
                        {
                            // The first record will be the first day on the chart.
                            if (intCounter1 == 0)
                            {
                                strFirstDay = item.timestamp;
                            }

                            intCounter1++;

                            DateTime dtPublishDT = Convert.ToDateTime(item.timestamp);

                            if (dtDate.Date == dtPublishDT.Date)
                            {
                                intDaycount++;
                            }
                            else
                            {
                                intCounter2++;
                                DataRow row = tablePubDates.NewRow();
                                row["Date"] = dtDate.ToString();
                                row["Count"] = intDaycount;
                                tablePubDates.Rows.Add(row);

                                intDaycount = 0;
                                dtDate = dtPublishDT;

                            }


                            // The last record will be the last day on the chart.
                            strLastDay = item.timestamp;
                        }



                    }

                    fileBlob.Delete();

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
                return;
            }

            


        }

    }

    public class CONTACT
    {
        public string lastSeen { get; set; }
        public int port { get; set; }
        public string address { get; set; }
        public string userAgent { get; set; }
        public string protocol { get; set; }
        public string lastTimeout { get; set; }
        public double timeoutRate { get; set; }
        public string nodeID { get; set; }
        public string responseTime { get; set; }
    }

    public class publish
    {
        public string level { get; set; }
        public string message { get; set; }
        public agent agent {get; set;}
        public string timestamp { get; set; }

    }

    public class agent
    {
        public string userAgent { get; set; }
        public string protocol { get; set; }
        public string address { get; set; }
        public int port { get; set; }
        public string nodeID { get; set; }
        public int lastSeen { get; set; }

    }

    // {"level":"info","message":"sending PUBLISH message to {\"userAgent\":\"6.3.2\",\"protocol\":\"1.1.0\",\"address\":\"81.30.220.183\",\"port\":53394,
    // \"nodeID\":\"da658cc985c6a51b98b608a6b53bbdc09bf6611b\",\"lastSeen\":1494624166172}","timestamp":"2017-05-12T21:23:26.569Z"}

}