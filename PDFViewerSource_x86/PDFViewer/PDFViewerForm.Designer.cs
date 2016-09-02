namespace PDFViewer
{
    partial class PDFViewerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tsbOpen = new System.Windows.Forms.ToolStripButton();
            this.tsbPrePage = new System.Windows.Forms.ToolStripButton();
            this.tstbPageIndex = new System.Windows.Forms.ToolStripTextBox();
            this.tslblPageAll = new System.Windows.Forms.ToolStripLabel();
            this.tsbNextPage = new System.Windows.Forms.ToolStripButton();
            this.tsbZoomOut = new System.Windows.Forms.ToolStripButton();
            this.tsbZoomIn = new System.Windows.Forms.ToolStripButton();
            this.ddlZoom = new System.Windows.Forms.ToolStripComboBox();
            this.tsbClockWise = new System.Windows.Forms.ToolStripButton();
            this.tsbCounterClockWise = new System.Windows.Forms.ToolStripButton();
            this.pnlViewer = new System.Windows.Forms.Panel();
            this.pbViewer = new System.Windows.Forms.PictureBox();
            this.pnlForm = new System.Windows.Forms.Panel();
            this.timerPrtScreen = new System.Windows.Forms.Timer(this.components);
            this.tsMain.SuspendLayout();
            this.pnlViewer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbViewer)).BeginInit();
            this.pnlForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsMain
            // 
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbOpen,
            this.tsbPrePage,
            this.tstbPageIndex,
            this.tslblPageAll,
            this.tsbNextPage,
            this.tsbZoomOut,
            this.tsbZoomIn,
            this.ddlZoom,
            this.tsbClockWise,
            this.tsbCounterClockWise});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(584, 25);
            this.tsMain.TabIndex = 0;
            // 
            // tsbOpen
            // 
            this.tsbOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOpen.Image = global::PDFViewer.Properties.Resources.dmdskres_373_9_16x16x32;
            this.tsbOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpen.Name = "tsbOpen";
            this.tsbOpen.Size = new System.Drawing.Size(23, 22);
            this.tsbOpen.Text = "Open";
            this.tsbOpen.Click += new System.EventHandler(this.tsbOpen_Click);
            // 
            // tsbPrePage
            // 
            this.tsbPrePage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPrePage.Image = global::PDFViewer.Properties.Resources.netshell_21611_1_16x16x32;
            this.tsbPrePage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPrePage.Name = "tsbPrePage";
            this.tsbPrePage.Size = new System.Drawing.Size(23, 22);
            this.tsbPrePage.Text = "Previous Page";
            this.tsbPrePage.Click += new System.EventHandler(this.tsbPrePage_Click);
            // 
            // tstbPageIndex
            // 
            this.tstbPageIndex.Name = "tstbPageIndex";
            this.tstbPageIndex.Size = new System.Drawing.Size(30, 25);
            this.tstbPageIndex.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tstbPageIndex_KeyDown);
            this.tstbPageIndex.MouseLeave += new System.EventHandler(this.tstbPageIndex_MouseLeave);
            // 
            // tslblPageAll
            // 
            this.tslblPageAll.Name = "tslblPageAll";
            this.tslblPageAll.Size = new System.Drawing.Size(18, 22);
            this.tslblPageAll.Text = "/0";
            // 
            // tsbNextPage
            // 
            this.tsbNextPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNextPage.Image = global::PDFViewer.Properties.Resources.netshell_1611_1_16x16x32;
            this.tsbNextPage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNextPage.Name = "tsbNextPage";
            this.tsbNextPage.Size = new System.Drawing.Size(23, 22);
            this.tsbNextPage.Text = "Next Page";
            this.tsbNextPage.Click += new System.EventHandler(this.tsbNextPage_Click);
            // 
            // tsbZoomOut
            // 
            this.tsbZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbZoomOut.Image = global::PDFViewer.Properties.Resources.ZoomOut;
            this.tsbZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbZoomOut.Name = "tsbZoomOut";
            this.tsbZoomOut.Size = new System.Drawing.Size(23, 22);
            this.tsbZoomOut.Text = "ZoomOut";
            this.tsbZoomOut.Click += new System.EventHandler(this.tsbZoomOut_Click);
            // 
            // tsbZoomIn
            // 
            this.tsbZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbZoomIn.Image = global::PDFViewer.Properties.Resources.ZoomIn;
            this.tsbZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbZoomIn.Name = "tsbZoomIn";
            this.tsbZoomIn.Size = new System.Drawing.Size(23, 22);
            this.tsbZoomIn.Text = "ZoomIn";
            this.tsbZoomIn.Click += new System.EventHandler(this.tsbZoomIn_Click);
            // 
            // ddlZoom
            // 
            this.ddlZoom.AutoSize = false;
            this.ddlZoom.Items.AddRange(new object[] {
            "25%",
            "50%",
            "75%",
            "100%",
            "125%",
            "150%",
            "200%",
            "400%"});
            this.ddlZoom.Name = "ddlZoom";
            this.ddlZoom.Size = new System.Drawing.Size(100, 23);
            this.ddlZoom.SelectedIndexChanged += new System.EventHandler(this.ddlZoom_SelectedIndexChanged);
            this.ddlZoom.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ddlZoom_KeyDown);
            this.ddlZoom.MouseLeave += new System.EventHandler(this.ddlZoom_MouseLeave);
            // 
            // tsbClockWise
            // 
            this.tsbClockWise.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbClockWise.Image = global::PDFViewer.Properties.Resources.clockwise90;
            this.tsbClockWise.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClockWise.Name = "tsbClockWise";
            this.tsbClockWise.Size = new System.Drawing.Size(23, 22);
            this.tsbClockWise.Text = "ClockWise 90";
            this.tsbClockWise.Click += new System.EventHandler(this.tsbClockWise_Click);
            // 
            // tsbCounterClockWise
            // 
            this.tsbCounterClockWise.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCounterClockWise.Image = global::PDFViewer.Properties.Resources.counterclockwise90;
            this.tsbCounterClockWise.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCounterClockWise.Name = "tsbCounterClockWise";
            this.tsbCounterClockWise.Size = new System.Drawing.Size(23, 22);
            this.tsbCounterClockWise.Text = "CounterClockWise 90";
            this.tsbCounterClockWise.Click += new System.EventHandler(this.tsbCounterClockWise_Click);
            // 
            // pnlViewer
            // 
            this.pnlViewer.Controls.Add(this.pbViewer);
            this.pnlViewer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pnlViewer.Location = new System.Drawing.Point(3, 3);
            this.pnlViewer.Name = "pnlViewer";
            this.pnlViewer.Size = new System.Drawing.Size(570, 425);
            this.pnlViewer.TabIndex = 1;
            this.pnlViewer.Click += new System.EventHandler(this.pnlViewer_Click);
            // 
            // pbViewer
            // 
            this.pbViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbViewer.Location = new System.Drawing.Point(0, 0);
            this.pbViewer.Name = "pbViewer";
            this.pbViewer.Size = new System.Drawing.Size(570, 425);
            this.pbViewer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbViewer.TabIndex = 0;
            this.pbViewer.TabStop = false;
            this.pbViewer.Click += new System.EventHandler(this.pbViewer_Click);
            // 
            // pnlForm
            // 
            this.pnlForm.AutoScroll = true;
            this.pnlForm.Controls.Add(this.pnlViewer);
            this.pnlForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlForm.Location = new System.Drawing.Point(0, 25);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Size = new System.Drawing.Size(584, 437);
            this.pnlForm.TabIndex = 0;
            this.pnlForm.Click += new System.EventHandler(this.pnlForm_Click);
            // 
            // PDFViewerForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(584, 462);
            this.Controls.Add(this.pnlForm);
            this.Controls.Add(this.tsMain);
            this.KeyPreview = true;
            this.Name = "PDFViewerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PDF Viewer";
            this.Activated += new System.EventHandler(this.PDFViewerForm_Activated);
            this.Deactivate += new System.EventHandler(this.PDFViewerForm_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PDFViewerForm_FormClosed);
            this.Load += new System.EventHandler(this.PDFViewerForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PDFViewerForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PDFViewerForm_KeyUp);
            this.Move += new System.EventHandler(this.PDFViewerForm_Move);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.pnlViewer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbViewer)).EndInit();
            this.pnlForm.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.ToolStripButton tsbOpen;
        private System.Windows.Forms.ToolStripButton tsbPrePage;
        private System.Windows.Forms.ToolStripTextBox tstbPageIndex;
        private System.Windows.Forms.ToolStripLabel tslblPageAll;
        private System.Windows.Forms.ToolStripButton tsbNextPage;
        private System.Windows.Forms.ToolStripButton tsbZoomIn;
        private System.Windows.Forms.ToolStripButton tsbZoomOut;
        private System.Windows.Forms.ToolStripButton tsbClockWise;
        private System.Windows.Forms.ToolStripButton tsbCounterClockWise;
        private System.Windows.Forms.Panel pnlViewer;
        private System.Windows.Forms.PictureBox pbViewer;
        private System.Windows.Forms.ToolStripComboBox ddlZoom;
        private System.Windows.Forms.Panel pnlForm;
        private System.Windows.Forms.Timer timerPrtScreen;
    }
}

