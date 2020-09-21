namespace ScatterChartFormatRulesSample {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.dashboardDesigner1 = new DevExpress.DashboardWin.DashboardDesigner();
            this.svgImageCollection1 = new DevExpress.Utils.SvgImageCollection(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dashboardDesigner1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgImageCollection1)).BeginInit();
            this.SuspendLayout();
            // 
            // dashboardDesigner1
            // 
            this.dashboardDesigner1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dashboardDesigner1.Appearance.Options.UseBackColor = true;
            this.dashboardDesigner1.AsyncMode = true;
            this.dashboardDesigner1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dashboardDesigner1.Location = new System.Drawing.Point(0, 0);
            this.dashboardDesigner1.Name = "dashboardDesigner1";
            this.dashboardDesigner1.Size = new System.Drawing.Size(1181, 592);
            this.dashboardDesigner1.TabIndex = 0;
            // 
            // svgImageCollection1
            // 
            this.svgImageCollection1.Add("chartrangearea", "image://svgimages/dashboards/chartrangearea.svg");
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1181, 592);
            this.Controls.Add(this.dashboardDesigner1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dashboardDesigner1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgImageCollection1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.DashboardWin.DashboardDesigner dashboardDesigner1;
        private DevExpress.Utils.SvgImageCollection svgImageCollection1;
    }
}

