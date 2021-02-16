
namespace ML_GUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.start_button = new System.Windows.Forms.Button();
            this.snapshot_button = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.predict_button = new System.Windows.Forms.Button();
            this.open_file_button = new System.Windows.Forms.Button();
            this.train_button = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pictureBox1.Location = new System.Drawing.Point(17, 88);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(439, 382);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // start_button
            // 
            this.start_button.Location = new System.Drawing.Point(511, 88);
            this.start_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.start_button.Name = "start_button";
            this.start_button.Size = new System.Drawing.Size(111, 38);
            this.start_button.TabIndex = 1;
            this.start_button.Text = "Start";
            this.start_button.UseVisualStyleBackColor = true;
            this.start_button.Click += new System.EventHandler(this.start_button_Click);
            // 
            // snapshot_button
            // 
            this.snapshot_button.Location = new System.Drawing.Point(511, 250);
            this.snapshot_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.snapshot_button.Name = "snapshot_button";
            this.snapshot_button.Size = new System.Drawing.Size(111, 38);
            this.snapshot_button.TabIndex = 2;
            this.snapshot_button.Text = "Snapshot";
            this.snapshot_button.UseVisualStyleBackColor = true;
            this.snapshot_button.Click += new System.EventHandler(this.snapshot_button_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pictureBox2.Location = new System.Drawing.Point(687, 88);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(439, 382);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // predict_button
            // 
            this.predict_button.Location = new System.Drawing.Point(511, 432);
            this.predict_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.predict_button.Name = "predict_button";
            this.predict_button.Size = new System.Drawing.Size(111, 38);
            this.predict_button.TabIndex = 5;
            this.predict_button.Text = "Predict";
            this.predict_button.UseVisualStyleBackColor = true;
            this.predict_button.Click += new System.EventHandler(this.predict_button_Click);
            // 
            // open_file_button
            // 
            this.open_file_button.Location = new System.Drawing.Point(1014, 480);
            this.open_file_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.open_file_button.Name = "open_file_button";
            this.open_file_button.Size = new System.Drawing.Size(111, 38);
            this.open_file_button.TabIndex = 6;
            this.open_file_button.Text = "Open File";
            this.open_file_button.UseVisualStyleBackColor = true;
            this.open_file_button.Click += new System.EventHandler(this.open_file_button_Click);
            // 
            // train_button
            // 
            this.train_button.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.train_button.Location = new System.Drawing.Point(17, 480);
            this.train_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.train_button.Name = "train_button";
            this.train_button.Size = new System.Drawing.Size(111, 38);
            this.train_button.TabIndex = 7;
            this.train_button.Text = "Train Model";
            this.train_button.UseVisualStyleBackColor = true;
            this.train_button.Click += new System.EventHandler(this.train_button_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(134, 480);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(321, 38);
            this.progressBar1.TabIndex = 8;
            this.progressBar1.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(140, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 54);
            this.label1.TabIndex = 9;
            this.label1.Text = "Webcam";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(780, 22);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(280, 54);
            this.label2.TabIndex = 10;
            this.label2.Text = "Image Preview";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(17, 557);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(1109, 47);
            this.label3.TabIndex = 11;
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1134, 600);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.train_button);
            this.Controls.Add(this.open_file_button);
            this.Controls.Add(this.predict_button);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.snapshot_button);
            this.Controls.Add(this.start_button);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(1156, 656);
            this.MinimumSize = new System.Drawing.Size(1156, 656);
            this.Name = "Form1";
            this.Text = "ML_GUI";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button start_button;
        private System.Windows.Forms.Button snapshot_button;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button predict_button;
        private System.Windows.Forms.Button open_file_button;
        private System.Windows.Forms.Button train_button;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

