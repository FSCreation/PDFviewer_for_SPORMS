﻿using CSharpKeyboardHook;
using Microsoft.InformationProtectionAndControl;
using Microsoft.VisualBasic;
using PDFLibNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections;

namespace PDFViewer
{
    public partial class PDFViewerForm : Form
    {
        PDFWrapper pdfDoc = null;
        //current page
        private static int currentPage = 0;
        //page count of document 
        private static int pageCount = 0;
        //current ratation
        private static int currentRatation = 0;
        //picture dpi
        private static int imageDPI = 90;
        //picture quality
        private static int imageQuality = 90;
        private static Image img = null;
        //current Zoom
        private static int currentZoom = 0;
        private Dictionary<int, double> zoomRate = new Dictionary<int, double>();
        //reconding a before value of VerticalScroll
        private static int VerticalScrollValue = -1;

        private Stream outPutStream = null;
        private string tempFile = string.Empty;

        //Hook management class
        private KeyboardHookLib _keyboardHook = null;

        private string pdfFileName = string.Empty;

        private bool rmsUserpEncrypt = true;

        //add 20160801
        //Active Flag 
        private bool formActive = false;

        private bool altKeyActive = false;
        
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        public PDFViewerForm()
        {
            InitializeComponent();
            //scroll bar MouseWheel
            this.pnlForm.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pnlForm_MouseWheel);

            //this.Resize += new System.EventHandler(this.PDFViewerForm_Resize);
        }

        public PDFViewerForm(string[] args)
        {
            InitializeComponent();
            //scroll bar MouseWheel
            this.pnlForm.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pnlForm_MouseWheel);

            //this.Resize += new System.EventHandler(this.PDFViewerForm_Resize);

            if (args.Length > 0)
            {
                pdfFileName = args[0];
            }
        }

        #region Toolbar Events


        /// <summary>
        /// Open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbOpen_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "PDF File (*.pdf)|*.pdf";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    FormInitial(dlg.FileName);
                }
            }
            catch (Exception ex)
            {
                throw;
                //MessageBox.Show(ex.ToString());
            }

        }

        /// <summary>
        /// Previous Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbPrePage_Click(object sender, EventArgs e)
        {
            try
            {
                if (pdfDoc != null && pdfDoc.PageCount > 0)
                {
                    if (currentPage == 0 || currentPage == 1)
                    {
                        return;
                    }
                    currentPage--;

                    tstbPageIndex.Text = currentPage.ToString();

                    SetPictureBoxImage();

                }
            }
            catch (Exception ex)
            {
                throw;
                //MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Next Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbNextPage_Click(object sender, EventArgs e)
        {
            try
            {
                if (pdfDoc != null && pdfDoc.PageCount > 0)
                {
                    if (currentPage == 0 || currentPage == pageCount)
                    {
                        return;
                    }
                    currentPage++;

                    tstbPageIndex.Text = currentPage.ToString();

                    SetPictureBoxImage();

                }
            }
            catch (Exception ex)
            {
                throw;
                //MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Rotate 90 degrees clockwise
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClockWise_Click(object sender, EventArgs e)
        {
            try
            {
                if (pdfDoc != null && pdfDoc.PageCount > 0)
                {
                    currentRatation = GetRatation(90);
                    int w = Convert.ToInt32(img.Width * zoomRate[currentZoom]);
                    int h = Convert.ToInt32(img.Height * zoomRate[currentZoom]);
                    Image imgNew = ResizeImage((Bitmap)img, w, h);
                    imgNew = SetCurrentRatationImage(imgNew);
                    pbViewer.Image = imgNew;

                    FitPnlViewerSize(imgNew);
                    SetPnlViewerLocation();
                }
            }
            catch (Exception ex)
            {
                throw;
                //MessageBox.Show(ex.ToString());
            }

        }

        /// <summary>
        /// Rotate 90 degrees Counterclockwise
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbCounterClockWise_Click(object sender, EventArgs e)
        {
            try
            {
                if (pdfDoc != null && pdfDoc.PageCount > 0)
                {
                    currentRatation = GetRatation(270);
                    int w = Convert.ToInt32(img.Width * zoomRate[currentZoom]);
                    int h = Convert.ToInt32(img.Height * zoomRate[currentZoom]);
                    Image imgNew = ResizeImage((Bitmap)img, w, h);
                    imgNew = SetCurrentRatationImage(imgNew);
                    pbViewer.Image = imgNew;

                    FitPnlViewerSize(imgNew);
                    SetPnlViewerLocation();
                }
            }
            catch (Exception ex)
            {
                throw;
                //MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// ZoomIn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbZoomIn_Click(object sender, EventArgs e)
        {
            try
            {
                if (pdfDoc != null && pdfDoc.PageCount > 0)
                {
                    if (currentZoom == 4)
                    {
                        return;
                    }
                    currentZoom++;

                    int w = Convert.ToInt32(img.Width * zoomRate[currentZoom]);
                    int h = Convert.ToInt32(img.Height * zoomRate[currentZoom]);
                    Image imgNew = ResizeImage((Bitmap)img, w, h);
                    imgNew = SetCurrentRatationImage(imgNew);
                    pbViewer.Image = imgNew;


                    foreach (string item in ddlZoom.Items)
                    {
                        if (item.Equals((zoomRate[currentZoom] * 100).ToString() + "%"))
                        {
                            ddlZoom.SelectedIndex = currentZoom - (-3);
                        }
                    }

                    FitPnlViewerSize(imgNew);
                    SetPnlViewerLocation();
                }
            }
            catch (Exception ex)
            {
                throw;
                //MessageBox.Show(ex.ToString());
            }

        }

        /// <summary>
        /// ZoomOut
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbZoomOut_Click(object sender, EventArgs e)
        {
            try
            {
                if (pdfDoc != null && pdfDoc.PageCount > 0)
                {
                    if (currentZoom == -3)
                    {
                        return;
                    }
                    currentZoom--;

                    int w = Convert.ToInt32(img.Width * zoomRate[currentZoom]);
                    int h = Convert.ToInt32(img.Height * zoomRate[currentZoom]);

                    Image imgNew = ResizeImage((Bitmap)img, w, h);
                    imgNew = SetCurrentRatationImage(imgNew);
                    pbViewer.Image = imgNew;

                    foreach (string item in ddlZoom.Items)
                    {
                        if (item.Equals((zoomRate[currentZoom] * 100).ToString() + "%"))
                        {
                            ddlZoom.SelectedIndex = currentZoom - (-3);
                        }
                    }

                    FitPnlViewerSize(imgNew);
                    SetPnlViewerLocation();
                }
            }
            catch (Exception ex)
            {
                throw;
                //MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Jump to a specific page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tstbPageIndex_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (pdfDoc != null && pdfDoc.PageCount > 0 && !IsDisposed)
                    {
                        int pageIndex = -1;
                        if (!int.TryParse(tstbPageIndex.Text, out pageIndex))
                        {
                            return;
                        }
                        if (pageIndex > 0 && pageIndex <= pdfDoc.PageCount)
                        {
                            currentPage = pageIndex;
                        }
                        SetPictureBoxImage();
                    }
                    this.pnlViewer.Location = new Point(this.pnlViewer.Location.X, this.pnlViewer.Location.Y - this.pnlForm.VerticalScroll.Value);
                }
            }
            catch (Exception ex)
            {
                throw;
                //MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// Zoom rate changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddlZoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pdfDoc != null && pdfDoc.PageCount > 0 && img != null)
                {
                    int index = ((ToolStripComboBox)(sender)).SelectedIndex;
                    currentZoom = index + (-3);

                    int w = Convert.ToInt32(img.Width * zoomRate[currentZoom]);
                    int h = Convert.ToInt32(img.Height * zoomRate[currentZoom]);

                    Image imgNew = ResizeImage((Bitmap)img, w, h);
                    imgNew = SetCurrentRatationImage(imgNew);
                    pbViewer.Image = imgNew;


                    FitPnlViewerSize(imgNew);
                    SetPnlViewerLocation();
                }
            }
            catch (Exception ex)
            {
                throw;
                //MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// Custom changed zoom rate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddlZoom_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (pdfDoc != null && pdfDoc.PageCount > 0 && img != null)
                    {
                        string str = ddlZoom.Text;
                        string zoomStr = str.Contains("%") ? str.Substring(0, str.IndexOf("%")) : str;
                        int tempZoom = 100;
                        if (!int.TryParse(zoomStr, out tempZoom))
                        {
                            ddlZoom.Text = (zoomRate[currentZoom] * 100).ToString() + "%";
                            return;
                        }
                        if (tempZoom < 25)
                        {
                            tempZoom = 25;
                        }
                        else if (tempZoom > 400)
                        {
                            tempZoom = 400;
                        }
                        else
                        {
                            for (int i = -3; i <= 4; i++)
                            {
                                if (tempZoom < zoomRate[i] * 100)
                                {
                                    break;
                                }
                                currentZoom = i;
                            }
                        }

                        int w = Convert.ToInt32(img.Width * (tempZoom * 0.01));
                        int h = Convert.ToInt32(img.Height * (tempZoom * 0.01));

                        Image imgNew = ResizeImage((Bitmap)img, w, h);
                        imgNew = SetCurrentRatationImage(imgNew);
                        pbViewer.Image = imgNew;

                        ddlZoom.Text = tempZoom + "%";

                        FitPnlViewerSize(imgNew);
                        SetPnlViewerLocation();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
                //MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region ScrollBar Events

        //[DllImport("user32.dll", EntryPoint = "GetScrollPos")]
        //public static extern int GetScrollPos(
        // int hwnd,
        // int nBar
        //);

        /// <summary>
        /// pnlForm_MouseWheel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pnlForm_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                //int i = GetScrollPos((int)this.pnlForm.Handle, 1);

                //previous page
                if ((this.pnlForm.VerticalScroll.Visible == false || VerticalScrollValue == 0) && e.Delta == 120)
                {
                    if (pdfDoc != null && pdfDoc.PageCount > 0)
                    {
                        if (currentPage == 0 || currentPage == 1)
                        {
                            return;
                        }
                        currentPage--;

                        tstbPageIndex.Text = currentPage.ToString();

                        SetPictureBoxImage();
                    }
                    return;
                }

                //next page
                if ((this.pnlForm.VerticalScroll.Visible == false || VerticalScrollValue == this.pnlForm.VerticalScroll.Value) && e.Delta == -120)
                {
                    if (pdfDoc != null && pdfDoc.PageCount > 0)
                    {
                        if (currentPage == 0 || currentPage == pageCount)
                        {
                            return;
                        }
                        currentPage++;

                        tstbPageIndex.Text = currentPage.ToString();

                        SetPictureBoxImage();
                    }
                    this.pnlViewer.Location = new Point(this.pnlViewer.Location.X, this.pnlViewer.Location.Y - this.pnlForm.VerticalScroll.Value);
                    return;
                }

                VerticalScrollValue = this.pnlForm.VerticalScroll.Value;
                this.pnlForm.VerticalScroll.Value += 1;
                this.Refresh();
                this.Invalidate();
                this.Update();

            }
            catch (Exception ex)
            {
                throw;
                //MessageBox.Show(ex.ToString());
            }
        }

        private void pnlForm_Click(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void pbViewer_Click(object sender, EventArgs e)
        {
            this.pnlForm.Focus();
        }

        private void pnlViewer_Click(object sender, EventArgs e)
        {
            this.pnlForm.Focus();
        }

        #endregion

        #region Menthod


        /// <summary>
        /// Form initialization
        /// </summary>
        /// <param name="fileName">fileName</param>
        private void FormInitial(string fileName)
        {
            rmsUserpEncrypt = true;

            if (outPutStream != null)
            {
                outPutStream.Close();
                outPutStream.Dispose();
            }

            if (!tempFile.Equals(string.Empty))
            {
                if (File.Exists(tempFile + ".tmp"))
                {
                    WipeFile(tempFile + ".tmp", 1);
                }
                tempFile = string.Empty;
            }

            if (pdfDoc != null)
            {
                pdfDoc.Dispose();
                pdfDoc = null;
            }
            pdfDoc = new PDFWrapper();

            if (!LoadFileByStream(fileName))
            {
                return;
            }
            //set zoomRate
            if (zoomRate.Count == 0)
            {
                zoomRate.Add(-3, 0.25);
                zoomRate.Add(-2, 0.5);
                zoomRate.Add(-1, 0.75);
                zoomRate.Add(0, 1);
                zoomRate.Add(1, 1.25);
                zoomRate.Add(2, 1.5);
                zoomRate.Add(3, 2);
                zoomRate.Add(4, 4);
            }

            foreach (string item in ddlZoom.Items)
            {
                if (item.Equals((zoomRate[currentZoom] * 100).ToString() + "%"))
                {
                    ddlZoom.SelectedIndex = currentZoom - (-3);
                }
            }

            currentPage = pdfDoc.CurrentPage;
            pageCount = pdfDoc.PageCount;
            tslblPageAll.Text = "/" + pageCount.ToString();
            tstbPageIndex.Text = currentPage.ToString();

            SetPictureBoxImage();

            this.WindowState = FormWindowState.Maximized;
            this.pnlForm.Focus();
        }

        /// <summary>
        /// Load pdf file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool LoadFile(string fileName)
        {
            bool isrmsProtect = true;
            byte[] license = null;

            try
            {
                //RMS化PDFファイルから、RMSライセンス情報と、暗号化された本文情報を分割する
                //RMS署名情報から、RMSサーバー情報を抽出する
                //RMSサーバーでの認証
                //RMSサーバーからRMSライセンスの取得
                license = SafeFileApiNativeMethods.IpcfGetSerializedLicenseFromFile(fileName);
            }
            catch (Exception ex)
            {
                isrmsProtect = false;
            }

            if (isrmsProtect)
            {
                try
                {
                    //SymmetricKeyCredential symmkey = new SymmetricKeyCredential();
                    //symmkey.AppPrincipalId = "0C5BDABD-CF4D-4FBB-BF4A-DD62BCF7E976";
                    //symmkey.Base64Key = "P@ssw0rd";
                    //symmkey.BposTenantId = "microsoftrmsonline@99db46e5-850f-4c66-9c21-3f86f9bbad94.rms.ap.aadrm.com";

                    SymmetricKeyCredential symmkey = null;

                    //RMSライセンスから、復号鍵の抽出
                    SafeInformationProtectionKeyHandle keyHandle = SafeNativeMethods.IpcGetKey(license, false, false, true, this);
                    //symmkey = (SymmetricKeyCredential)keyHandle;

                    //RMSライセンスから、権利リストの抽出
                    //Collection<UserRights> userRights = new Collection<UserRights>();
                    //userRights = SafeNativeMethods.IpcGetSerializedLicenseUserRightsList(license, keyHandle);

                    bool accessGranted = SafeNativeMethods.IpcAccessCheck(keyHandle, "VIEW");

                    if (accessGranted)
                    {
                        SafeFileApiNativeMethods.IpcfDecryptFile(fileName,
                            SafeFileApiNativeMethods.DecryptFlags.IPCF_DF_FLAG_DEFAULT,
                            false,
                            false,
                            true,
                            this,
                            symmkey);
                    }

                    //使用権限が正しく設定されていません
                    //ConnectionInfo connectionInfo = SafeNativeMethods.IpcGetSerializedLicenseConnectionInfo(license);
                    //System.Collections.ObjectModel.Collection<TemplateIssuer> templateIssuerList = SafeNativeMethods.IpcGetTemplateIssuerList(connectionInfo, false, false, false, false, this, symmkey);
                    //TemplateIssuer templateIssuer = templateIssuerList[0];
                    //SafeInformationProtectionLicenseHandle licenseHandle = SafeNativeMethods.IpcCreateLicenseFromScratch(templateIssuer);
                    //SafeFileApiNativeMethods.IpcfEncryptFile(fileName, licenseHandle, SafeFileApiNativeMethods.EncryptFlags.IPCF_EF_FLAG_DEFAULT, false, false, false, this, symmkey);

                    //テンプレートは管理者によって作成されていません
                    //TemplateInfo templateInfo = SafeNativeMethods.IpcGetSerializedLicenseDescriptor(license, keyHandle, System.Globalization.CultureInfo.CurrentCulture);
                    //SafeFileApiNativeMethods.IpcfEncryptFile(fileName, templateInfo.TemplateId, SafeFileApiNativeMethods.EncryptFlags.IPCF_EF_FLAG_DEFAULT, false, false, true, this, null);

                }
                catch (InformationProtectionException ex)
                {
                    isrmsProtect = false;
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK);
                }
                catch (Exception ex)
                {
                    isrmsProtect = false;
                }
            }



            try
            {
                pdfDoc.LoadPDF(fileName);

                return true;
            }
            catch (System.Security.SecurityException sex)
            {
                String password = Interaction.InputBox("Please enter the document password:", "Document Password", "");
                if (password.Equals(string.Empty))
                {
                    return false;
                }

                if (pdfDoc != null)
                {
                    pdfDoc.Dispose();
                    pdfDoc = null;
                }
                pdfDoc = new PDFWrapper();
                pdfDoc.UserPassword = password;
                return LoadFile(fileName);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Load pdf file by stream
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool LoadFileByStream(string fileName)
        {
            bool isrmsProtect = true;
            Stream stream = null;
            byte[] license = null;
            string rmsUserPassword = string.Empty;


            if (rmsUserpEncrypt)
            {
                try
                {
                    //RMS化PDFファイルから、RMSライセンス情報と、暗号化された本文情報を分割する
                    //RMS署名情報から、RMSサーバー情報を抽出する
                    //RMSサーバーでの認証
                    //RMSサーバーからRMSライセンスの取得
                    license = SafeFileApiNativeMethods.IpcfGetSerializedLicenseFromFile(fileName);
                }
                catch (Exception ex)
                {
                    isrmsProtect = false;
                }
            }

            if (isrmsProtect && rmsUserpEncrypt)
            {
                try
                {
                    rmsUserPassword = GenerateRandom(32);

                    //RMSライセンスから、復号鍵の抽出
                    SafeInformationProtectionKeyHandle keyHandle = SafeNativeMethods.IpcGetKey(license, false, false, true, this);

                    //RMSライセンスから、権利リストの抽出
                    //Collection<UserRights> userRights = new Collection<UserRights>();
                    //userRights = SafeNativeMethods.IpcGetSerializedLicenseUserRightsList(license, keyHandle);

                    bool accessGranted = SafeNativeMethods.IpcAccessCheck(keyHandle, "VIEW");

                    //本文情報を復号鍵で、復号
                    tempFile = GenerateRandom(10);

                    //一時フォルダ作成 add kondo
                    System.IO.Directory.CreateDirectory(Path.GetTempPath() + @"PDFViewer\");

                    tempFile = Path.GetTempPath() + @"PDFViewer\" + tempFile;

                    Stream outPutRmsStream = new FileStream(tempFile, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                    stream = new FileStream(fileName, FileMode.Open);
                    if (accessGranted)
                    {
                        SafeFileApiNativeMethods.IpcfDecryptFileStream(stream, fileName,
                            SafeFileApiNativeMethods.DecryptFlags.IPCF_DF_FLAG_DEFAULT, false,
                            false, false, this, ref outPutRmsStream);
                    }

                    outPutRmsStream.Close();
                    outPutRmsStream.Dispose();

                    PdfReader reader = new PdfReader(tempFile);
                    outPutStream = new FileStream(tempFile + ".tmp", FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                    PdfEncryptor.Encrypt(reader, outPutStream, false, rmsUserPassword, "", 0);

                    rmsUserpEncrypt = false;
                    reader.Close();
                    reader.Dispose();
                    File.Delete(tempFile);

                }
                catch (InformationProtectionException ex)
                {
                    //DirectoryDelete MSIPC
                    DeleteDirectorySelect(true);

                    isrmsProtect = false;
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK);
                }
                catch (Exception ex)
                {
                    isrmsProtect = false;
                }
            }


            try
            {
                if (isrmsProtect)
                {
                    pdfDoc.LoadPDF(tempFile + ".tmp");
                }
                else
                {
                    pdfDoc.LoadPDF(fileName);
                }
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
                return true;
            }
            catch (System.Security.SecurityException sex)
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }

                if (pdfDoc != null)
                {
                    pdfDoc.Dispose();
                    pdfDoc = null;
                }
                pdfDoc = new PDFWrapper();

                if (!rmsUserpEncrypt)
                {
                    pdfDoc.UserPassword = rmsUserPassword;
                }
                else
                {
                    String password = Interaction.InputBox("Please enter the document password:", "Document Password", "");
                    if (password.Equals(string.Empty))
                    {
                        return false;
                    }
                    pdfDoc.UserPassword = password;
                }

                return LoadFileByStream(fileName);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                return false;
            }
        }


        /// <summary>
        /// Set image for PictureBox
        /// </summary>
        private void SetPictureBoxImage()
        {
            try
            {
                // 一時フォルダ存在チェック
                if (!System.IO.Directory.Exists(Path.GetTempPath() + @"PDFViewer\"))
                {
                    System.IO.Directory.CreateDirectory(Path.GetTempPath() + @"PDFViewer\");
                }

                string jpgFileName = Path.GetTempPath() + @"PDFViewer\" + "temp" + DateTime.Now.Millisecond;

                //Create current page
                pdfDoc.ExportJpg(jpgFileName, currentPage, currentPage, imageDPI, imageQuality);
                if (pdfDoc.IsJpgBusy)
                {
                    System.Threading.Thread.Sleep(50);
                }

                do
                {
                    System.Threading.Thread.Sleep(100);
                }
                while (!File.Exists(jpgFileName));

                System.Threading.Thread.Sleep(100);
                Stream stream = new FileStream(jpgFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                img = new Bitmap(stream);
                img = ResizeImage((Bitmap)img, img.Width, img.Height);

                int w = Convert.ToInt32(img.Width * zoomRate[currentZoom]);
                int h = Convert.ToInt32(img.Height * zoomRate[currentZoom]);
                Image imgNew = ResizeImage((Bitmap)img, w, h);
                imgNew = SetCurrentRatationImage(imgNew);
                pbViewer.Image = imgNew;

                stream.Close();
                stream.Dispose();
                System.Threading.Thread.Sleep(100);
                File.Delete(jpgFileName);

                FitPnlViewerSize(imgNew);
                SetPnlViewerLocation();
            }
            catch (Exception ex)
            {
                throw;
                //MessageBox.Show(ex.ToString());
            }

        }

        /// <summary>
        /// Get page Ratation
        /// </summary>
        /// <param name="pageRotation"></param>
        /// <returns></returns>
        private static int GetRatation(int pageRotation)
        {
            try
            {
                int Ratation = currentRatation + pageRotation;

                if (Ratation >= 360)
                {
                    do
                    {
                        Ratation -= 360;
                    }
                    while (Ratation >= 360);
                }
                return Ratation;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Make the page match the current rotation
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private static Image SetCurrentRatationImage(Image imgRatation)
        {
            if (currentRatation == 90)
            {
                imgRatation.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
            else if (currentRatation == 180)
            {
                imgRatation.RotateFlip(RotateFlipType.Rotate180FlipNone);
            }
            else if (currentRatation == 270)
            {
                imgRatation.RotateFlip(RotateFlipType.Rotate270FlipNone);
            }
            return imgRatation;
        }


        /// <summary>
        /// Set Location for pnlViewer
        /// </summary>
        private void SetPnlViewerLocation()
        {
            if (this.Width - pnlViewer.Width > 0 && this.Height - tsMain.Size.Height - pnlViewer.Height > 0)
            {
                pnlViewer.Location = new Point((this.Width - pnlViewer.Width) / 2, (this.Height - tsMain.Size.Height - pnlViewer.Height) / 2);
            }
            else if (this.Width - pnlViewer.Width > 0 && this.Height - tsMain.Size.Height - pnlViewer.Height <= 0)
            {
                pnlViewer.Location = new Point((this.Width - pnlViewer.Width) / 2, tsMain.Size.Height);
            }
            else if (this.Width - pnlViewer.Width <= 0 && this.Height - tsMain.Size.Height - pnlViewer.Height > 0)
            {
                pnlViewer.Location = new Point(0, (this.Height - tsMain.Size.Height - pnlViewer.Height) / 2);
            }
            else
            {
                pnlViewer.Location = new Point(0, tsMain.Size.Height);
            }
        }

        /// <summary>
        /// pnlViewer fit image
        /// </summary>
        private void FitPnlViewerSize(Image img)
        {
            pnlViewer.Width = img.Width;
            pnlViewer.Height = img.Height;
        }


        /// <summary>
        /// Generate the specified size image
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="newW"></param>
        /// <param name="newH"></param>
        /// <returns></returns>
        public static Bitmap ResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                return null;
            }
        }


        /// <summary> 
        /// Client capture keyboard events
        /// </summary> 
        /// <param name="hookStruct">Key information transmitted by Hook program</param> 
        /// <param name="handle">interception</param>
        public void KeyPress(KeyboardHookLib.HookStruct hookStruct, out bool handle)
        {
            IntPtr hWnd = GetForegroundWindow();
            int id;
            // ウィンドウハンドルからプロセスIDを取得
            GetWindowThreadProcessId(hWnd, out id);
            Process process = Process.GetProcessById(id);

            KeyPreview = true;

            handle = false;

            if (hookStruct.vkCode == (int)Keys.Menu)
            {
                altKeyActive = true;

            }
            if (hookStruct.vkCode == 164 || hookStruct.vkCode == 165)
            {
                altKeyActive = true;

            }
            if (formActive == false)
            {
                if (altKeyActive == false && hookStruct.vkCode == (int)Keys.PrintScreen)
                {
                    //handle = true;
                    if (Clipboard.ContainsImage())
                    {
                        Clipboard.SetDataObject(new DataObject());
                        altKeyActive = false;
                    }
                }
                if (altKeyActive == true && process.ProcessName == "explorer")
                {
                    if (Clipboard.ContainsImage())
                    {
                        Clipboard.SetDataObject(new DataObject());
                        altKeyActive = false;
                    }
                }

            }
            else
            {


                if (Clipboard.ContainsImage())
                {
                    Clipboard.SetDataObject(new DataObject());
                    altKeyActive = false;
                }


            }

        }


        #endregion

        #region Form Events

        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PDFViewerForm_Load(object sender, EventArgs e)
        {
            KeyPreview = true;
            if (Clipboard.ContainsImage())
            {
                Clipboard.Clear();
            }

            handle = this.Handle;

            timerPrtScreen.Enabled = true;
            timerPrtScreen.Interval = 500;

            //Instal Hook
            _keyboardHook = new KeyboardHookLib();
            _keyboardHook.InstallHook(this.KeyPress);

            //DirectoryDelete Temp
            DeleteDirectorySelect(false);

            if (!pdfFileName.Equals(string.Empty))
            {
                FormInitial(pdfFileName);
            }
        }

        private void ddlZoom_MouseLeave(object sender, EventArgs e)
        {
            this.pnlForm.Focus();
        }

        private void tstbPageIndex_MouseLeave(object sender, EventArgs e)
        {
            this.pnlForm.Focus();
        }

        /// <summary>
        /// Form Closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PDFViewerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (outPutStream != null)
            {
                outPutStream.Close();
                outPutStream.Dispose();
            }

            if (!tempFile.Equals(string.Empty))
            {
                if (File.Exists(tempFile + ".tmp"))
                {
                    WipeFile(tempFile + ".tmp", 1);
                }
                tempFile = string.Empty;
            }
            //Cancel Hook
            if (_keyboardHook != null)
            {
                _keyboardHook.UninstallHook();
            }

            timerPrtScreen.Enabled = false;
            timerPrtScreen.Dispose();

            base.Dispose();
        }

        #endregion

        #region Drag/drop handler

        /// <summary>
        /// Form_DragDrop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_DragDrop(object sender, DragEventArgs e)
        {
            string[] files;

            // though you can drop a set of files, we only take the first
            files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files.Length > 0)
            {
                try
                {
                    if (!files[0].Substring(files[0].Length - 4, 4).ToLower().Equals(".pdf"))
                    {
                        MessageBox.Show("Not a pdf file, please reselect.", "Whoops!", MessageBoxButtons.OK);
                        return;
                    }
                    FormInitial(files[0]);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Whoops!", MessageBoxButtons.OK);
                }
            }
        }

        /// <summary>
        /// Form_DragEnter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        #endregion

        #region Generates a random code

        private static char[] constant =
        {
        '0','1','2','3','4','5','6','7','8','9',
        'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
        'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
        };

        public static string GenerateRandom(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(62)]);
            }
            return newRandom.ToString();
        }

        #endregion


        /// <summary>
        /// WipeFile
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="timesToWrite"></param>
        private void WipeFile(string filename, int timesToWrite)
        {
            try
            {
                if (File.Exists(filename))
                {
                    File.SetAttributes(filename, FileAttributes.Normal);
                    double sectors = Math.Ceiling(new FileInfo(filename).Length / 512.0);
                    byte[] dummyBuffer = new byte[512];
                    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                    FileStream inputStream = new FileStream(filename, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                    for (int currentPass = 0; currentPass < timesToWrite; currentPass++)
                    {
                        inputStream.Position = 0;
                        for (int sectorsWritten = 0; sectorsWritten < sectors; sectorsWritten++)
                        {
                            rng.GetBytes(dummyBuffer);
                            inputStream.Write(dummyBuffer, 0, dummyBuffer.Length);
                        }
                    }
                    inputStream.SetLength(0);
                    inputStream.Close();
                    DateTime dt = DateTime.Now;
                    File.SetCreationTime(filename, dt);
                    File.SetLastAccessTime(filename, dt);
                    File.SetLastWriteTime(filename, dt);
                    File.Delete(filename);
                }
            }
            catch (Exception e)
            {
            }
        }

        #region 追加対応
        /// <summary>
        /// 削除ディレクトリ
        /// </summary>
        /// <param name="flag">true:MSPIC False:PDFViewer</param>
        public void DeleteDirectorySelect(bool flag)
        {
            // ユーザ名取得
            string userName = Environment.UserName;
            // 出力メッセージ
            string msg = string.Empty;

            try
            {
                // [PDFViewer] or [MSPIC]
                if (flag)
                {
                    msg = "MSPIC";

                    if (System.IO.Directory.Exists(@"C:\Users\" + userName + @"\AppData\Local\Microsoft\MSIPC"))
                    {
                        this.DeleteDirectory(@"C:\Users\" + userName + @"\AppData\Local\Microsoft\MSIPC");
                    }
                }
                else
                {
                    msg = "PDFViewer";

                    if (System.IO.Directory.Exists(@"C:\Users\" + userName + @"\AppData\Local\Temp\PDFViewer"))
                    {
                        this.DeleteDirectory(@"C:\Users\" + userName + @"\AppData\Local\Temp\PDFViewer");
                    }
                }

            }
            catch (Exception e)
            {
            }

        }

        /// <summary>
        /// フォルダ削除（ReadOnlyでも削除）
        /// </summary>
        /// <param name="dir">削除するフォルダ</param>
        private void DeleteDirectory(string dir)
        {
            //DirectoryInfoオブジェクトの作成
            DirectoryInfo di = new DirectoryInfo(dir);

            //フォルダ以下のすべてのファイル、フォルダの属性を削除
            RemoveReadonlyAttribute(di);

            //フォルダを根こそぎ削除
            di.Delete(true);
        }

        private void RemoveReadonlyAttribute(DirectoryInfo dirInfo)
        {
            //基のフォルダの属性を変更
            if ((dirInfo.Attributes & FileAttributes.ReadOnly) ==
                FileAttributes.ReadOnly)
                dirInfo.Attributes = FileAttributes.Normal;

            //フォルダ内のすべてのファイルの属性を変更
            foreach (FileInfo fi in dirInfo.GetFiles())
                if ((fi.Attributes & FileAttributes.ReadOnly) ==
                    FileAttributes.ReadOnly)
                    fi.Attributes = FileAttributes.Normal;

            //サブフォルダの属性を回帰的に変更
            foreach (DirectoryInfo di in dirInfo.GetDirectories())
                RemoveReadonlyAttribute(di);

        }
        #endregion


        #region  PrtScreen 対応  2016/06/14

        /// <summary>
        /// Form Activated
        /// </summary>
        private void PDFViewerForm_Activated(object sender, EventArgs e)
        {
            //timerPrtScreen.Stop();

            ////Instal Hook
            if (_keyboardHook != null)
            {
                _keyboardHook.UninstallHook();
            }
            _keyboardHook = new KeyboardHookLib();
            _keyboardHook.InstallHook(this.KeyPress);
            formActive = true;
            altKeyActive = false;
        }

        /// <summary>
        /// Form Deactivate
        /// </summary>
        private void PDFViewerForm_Deactivate(object sender, EventArgs e)
        {
            ////Cancel Hook
            if (_keyboardHook != null)
            {
                _keyboardHook.UninstallHook();
            }
            //timerPrtScreen.Start();

            formActive = false;
            altKeyActive = false;

            _keyboardHook = new KeyboardHookLib();
            _keyboardHook.InstallHook(this.KeyPress);
        }

        #endregion

        #region 2016/06/15

        //[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        //public static extern IntPtr GetForegroundWindow();

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);


        IntPtr handle;

        public void GetFromSize(ref int width, ref int height, ref int x, ref int y)
        {
            RECT rect = new RECT();
            GetWindowRect(handle, ref rect);
            width = rect.Right - rect.Left;
            height = rect.Bottom - rect.Top;
            x = rect.Left;
            y = rect.Top;
        }

        private void PDFViewerForm_Move(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                Clipboard.Clear();
            }
        }

        #endregion

        #region 20160801


        private void PDFViewerForm_KeyDown(object sender, KeyEventArgs e)
        {
            //Activeの場合
            if (formActive == true)
            {
                // 下がったキーが[ Alt ]キーの場合
                if (e.Alt == true)
                {
                    altKeyActive = true;
                }
            }
        }


        private void PDFViewerForm_KeyUp(object sender, KeyEventArgs e)
        {
            //Activeの場合
            if (formActive == true)
            {

                // Upキーが[Alt  PrtScr ]キーの場合
                if (e.KeyCode == Keys.PrintScreen && altKeyActive == true)
                {
                    Clipboard.SetDataObject(new DataObject());
                }

            }

            //単体押下の場合
            // Upキーが[ PrtScr ]キーの場合
            if (e.KeyCode == Keys.PrintScreen && altKeyActive == false && formActive == true)
            {
                Clipboard.SetDataObject(new DataObject());
            }

        }
        #endregion
    }
}
