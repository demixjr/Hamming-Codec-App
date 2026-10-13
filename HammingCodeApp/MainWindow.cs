namespace HammingCodeApp
{
    public partial class MainForm : Form
    {
        private FileService? _fileService;
        private string? _filePath;
        private int _blockLength;

        public MainForm()
        {
            InitializeComponent();

            _blockLength = (int)numBlockSize.Value;
            txtFilePath.Text = string.Empty;

            lblStatus_Status_Ready();
        }

        private void lblStatus_Status_Ready()
        {
            lblStatus.Text = "Ready";
        }

        private void lblStatus_Status_Processing()
        {
            lblStatus.Text = "Processing...";
        }

        private bool ValidateFileSelection()
        {
            if (string.IsNullOrWhiteSpace(_filePath))
            {
                MessageBox.Show(
                    "Please select an existing file first.",
                    "File not selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            if (!File.Exists(_filePath))
            {
                MessageBox.Show(
                    "The selected file does not exist.",
                    "File not found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            return true;
        }

        private void InitializeFileService()
        {
            _blockLength = (int)numBlockSize.Value;

            if (string.IsNullOrWhiteSpace(_filePath))
            {
                return;
            }

            _fileService = new FileService(_filePath, _blockLength, new HammingCodec());
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(@"D:\hammingfiles"))
            {
                openFileDialog1.InitialDirectory = @"D:\hammingfiles";
            }
            else
            {
                openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _filePath = openFileDialog1.FileName;
                txtFilePath.Text = _filePath;

                try
                {
                    InitializeFileService();

                    rtbLog.Text =
                        "Selected file: " + _filePath;

                    lblStatus_Status_Ready();
                }
                catch (Exception ex)
                {
                    _fileService = null;
                    rtbLog.Text = "Error: " + ex.Message;

                    MessageBox.Show(
                        ex.Message,
                        "Initialization error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnEncode_Click(object sender, EventArgs e)
        {
            if (!ValidateFileSelection())
            {
                return;
            }

            lblStatus_Status_Processing();

            try
            {
                InitializeFileService();

                bool success = _fileService!.EncodeFile(_filePath!, _blockLength);

                if (success)
                {
                    string directory = Path.GetDirectoryName(Path.GetFullPath(_filePath!))!;

                    string fileName = Path.GetFileNameWithoutExtension(_filePath);

                    string encodedPath = Path.Combine(directory, fileName + "Encoded.txt");

                    rtbLog.Text = "File encoded successfully.\n" + "Output: " + encodedPath;
                }
                else
                {
                    rtbLog.Text = "File encoding failed.";
                }
            }
            catch (Exception ex)
            {
                rtbLog.Text = "Error: " + ex.Message;
            }
            finally
            {
                lblStatus_Status_Ready();
            }
        }

        private void btnDecode_Click(object sender, EventArgs e)
        {
            if (!ValidateFileSelection())
            {
                return;
            }

            lblStatus_Status_Processing();

            try
            {
                InitializeFileService();

                string report = _fileService!.DecodeFile(_filePath!);

                rtbLog.Text = report;
            }
            catch (Exception ex)
            {
                rtbLog.Text = "Error: " + ex.Message;
            }
            finally
            {
                lblStatus_Status_Ready();
            }
        }

        private void btnInjectErrors_Click(object sender, EventArgs e)
        {
            if (!ValidateFileSelection())
            {
                return;
            }

            lblStatus_Status_Processing();

            try
            {
                InitializeFileService();

                bool success = _fileService!.InjectFile(_filePath!);

                if (success)
                {
                    string directory = Path.GetDirectoryName(Path.GetFullPath(_filePath!))!;

                    string fileName = Path.GetFileNameWithoutExtension(_filePath);

                    string injectedPath = Path.Combine(directory, fileName + "Injected.txt");

                    rtbLog.Text = "Random bit errors injected successfully.\n" + "One bit was changed in each block.\n" + "Output: " + injectedPath;
                }
                else
                {
                    rtbLog.Text = "Error injection failed.";
                }
            }
            catch (Exception ex)
            {
                rtbLog.Text = "Error: " + ex.Message;
            }
            finally
            {
                lblStatus_Status_Ready();
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (!ValidateFileSelection())
            {
                return;
            }

            lblStatus_Status_Processing();

            try
            {
                InitializeFileService();

                bool success = _fileService!.RestoreFile(_filePath!);

                if (success)
                {
                    string fileName = Path.GetFileNameWithoutExtension(_filePath);

                    fileName = fileName.Replace("EncodedInjected", "");

                    fileName = fileName.Replace("Encoded", "");

                    fileName = fileName.Replace("Injected", "");

                    string directory = Path.GetDirectoryName(Path.GetFullPath(_filePath!))!;

                    string restoredPath = Path.Combine(directory, fileName + "Restored.txt");

                    rtbLog.Text = "File restored successfully.\n" + "Output: " + restoredPath;
                }
                else
                {
                    rtbLog.Text = "File restoration failed. Check the error output.";
                }
            }
            catch (Exception ex)
            {
                rtbLog.Text = "Error: " + ex.Message;
            }
            finally
            {
                lblStatus_Status_Ready();
            }
        }
    }
}
