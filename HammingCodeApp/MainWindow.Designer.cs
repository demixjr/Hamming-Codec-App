namespace HammingCodeApp
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();

            lblTitle = new Label();
            lblFile = new Label();
            txtFilePath = new TextBox();
            btnBrowse = new Button();

            lblBlockSize = new Label();
            numBlockSize = new NumericUpDown();

            btnEncode = new Button();
            btnDecode = new Button();
            btnInjectErrors = new Button();
            btnRestore = new Button();

            lblLog = new Label();
            rtbLog = new RichTextBox();

            lblStatus = new Label();

            ((System.ComponentModel.ISupportInitialize)numBlockSize).BeginInit();
            SuspendLayout();

            // 
            // openFileDialog1
            // 
            openFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.Title = "Select a text file";

            // 
            // saveFileDialog1
            // 
            saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.Title = "Save file";

            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.Location = new Point(24, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(370, 41);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Hamming Code App";

            // 
            // lblFile
            // 
            lblFile.AutoSize = true;
            lblFile.Location = new Point(27, 80);
            lblFile.Name = "lblFile";
            lblFile.Size = new Size(75, 20);
            lblFile.TabIndex = 1;
            lblFile.Text = "Input file:";

            // 
            // txtFilePath
            // 
            txtFilePath.Location = new Point(30, 105);
            txtFilePath.Name = "txtFilePath";
            txtFilePath.PlaceholderText = "Select a text file...";
            txtFilePath.ReadOnly = true;
            txtFilePath.Size = new Size(570, 27);
            txtFilePath.TabIndex = 2;

            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(615, 103);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(140, 32);
            btnBrowse.TabIndex = 3;
            btnBrowse.Text = "Browse...";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;

            // 
            // lblBlockSize
            // 
            lblBlockSize.AutoSize = true;
            lblBlockSize.Location = new Point(30, 155);
            lblBlockSize.Name = "lblBlockSize";
            lblBlockSize.Size = new Size(151, 20);
            lblBlockSize.TabIndex = 4;
            lblBlockSize.Text = "Data block size (bits):";

            // 
            // numBlockSize
            // 
            numBlockSize.Location = new Point(200, 153);
            numBlockSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numBlockSize.Maximum = new decimal(new int[] { 1024, 0, 0, 0 });
            numBlockSize.Value = new decimal(new int[] { 8, 0, 0, 0 });
            numBlockSize.Name = "numBlockSize";
            numBlockSize.Size = new Size(100, 27);
            numBlockSize.TabIndex = 5;

            // 
            // btnEncode
            // 
            btnEncode.Location = new Point(30, 205);
            btnEncode.Name = "btnEncode";
            btnEncode.Size = new Size(170, 42);
            btnEncode.TabIndex = 6;
            btnEncode.Text = "Encode File";
            btnEncode.UseVisualStyleBackColor = true;
            btnEncode.Click += btnEncode_Click;

            // 
            // btnDecode
            // 
            btnDecode.Location = new Point(215, 205);
            btnDecode.Name = "btnDecode";
            btnDecode.Size = new Size(170, 42);
            btnDecode.TabIndex = 7;
            btnDecode.Text = "Decode File";
            btnDecode.UseVisualStyleBackColor = true;
            btnDecode.Click += btnDecode_Click;

            // 
            // btnInjectErrors
            // 
            btnInjectErrors.Location = new Point(400, 205);
            btnInjectErrors.Name = "btnInjectErrors";
            btnInjectErrors.Size = new Size(170, 42);
            btnInjectErrors.TabIndex = 8;
            btnInjectErrors.Text = "Inject Errors";
            btnInjectErrors.UseVisualStyleBackColor = true;
            btnInjectErrors.Click += btnInjectErrors_Click;

            // 
            // btnRestore
            // 
            btnRestore.Location = new Point(585, 205);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(170, 42);
            btnRestore.TabIndex = 9;
            btnRestore.Text = "Restore File";
            btnRestore.UseVisualStyleBackColor = true;
            btnRestore.Click += btnRestore_Click;

            // 
            // lblLog
            // 
            lblLog.AutoSize = true;
            lblLog.Location = new Point(30, 270);
            lblLog.Name = "lblLog";
            lblLog.Size = new Size(91, 20);
            lblLog.TabIndex = 10;
            lblLog.Text = "Operation log:";

            // 
            // rtbLog
            // 
            rtbLog.Location = new Point(30, 295);
            rtbLog.Name = "rtbLog";
            rtbLog.ReadOnly = true;
            rtbLog.Font = new Font("Consolas", 10F);
            rtbLog.Size = new Size(725, 115);
            rtbLog.TabIndex = 11;
            rtbLog.Text = "Select a text file to begin";
            rtbLog.WordWrap = false;

            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(30, 420);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(45, 20);
            lblStatus.TabIndex = 12;
            lblStatus.Text = "Ready";

            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 465);
            MinimumSize = new Size(816, 504);
            Controls.Add(lblTitle);
            Controls.Add(lblFile);
            Controls.Add(txtFilePath);
            Controls.Add(btnBrowse);
            Controls.Add(lblBlockSize);
            Controls.Add(numBlockSize);
            Controls.Add(btnEncode);
            Controls.Add(btnDecode);
            Controls.Add(btnInjectErrors);
            Controls.Add(btnRestore);
            Controls.Add(lblLog);
            Controls.Add(rtbLog);
            Controls.Add(lblStatus);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hamming Code App";

            ((System.ComponentModel.ISupportInitialize)numBlockSize).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;

        private Label lblTitle;
        private Label lblFile;
        private TextBox txtFilePath;
        private Button btnBrowse;

        private Label lblBlockSize;
        private NumericUpDown numBlockSize;

        private Button btnEncode;
        private Button btnDecode;
        private Button btnInjectErrors;
        private Button btnRestore;

        private Label lblLog;
        private RichTextBox rtbLog;
        private Label lblStatus;
    }
}