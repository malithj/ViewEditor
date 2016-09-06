using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewEditor
{
    public partial class ViewEditor : Form
    {
        public string fileNameLoad
        {
            get;
            set;
        }

        public Bitmap image
        {
            get;
            set;
        }

        public double x_scale
        {
            get;
            set;
        }

        public double y_scale
        {
            get;
            set;
        }

        public ImageBox imageBox
        {
            get;
            set;
        }

        private int currentBrightnessLevel
        {
            get;
            set;
        }

        private int currentContrastLevel
        {
            get;
            set;
        }

        private Stack<Bitmap> undoStack
        {
            get;
            set;
        }

        private Stack<Bitmap> redoStack
        {
            get;
            set;
        }

        private Bitmap currentWorkableImage
        {
            get;
            set;
        }

        private Bitmap tempWorkableImage
        {
            get;
            set;
        }
    
        public ViewEditor()
        {
            undoStack = new Stack<Bitmap>();
            redoStack = new Stack<Bitmap>();
            currentBrightnessLevel = 0;
            currentContrastLevel = 0;
            fileNameLoad = null;
            imageBox= new ImageBox();
            InitializeComponent();
        }



        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void ResourcePanelLayout_Paint(object sender, PaintEventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadNewFile();
        }

        private void LoadNewFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            String pictureFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            ofd.InitialDirectory = pictureFolder;
            ofd.Multiselect = true;
            ofd.Filter = "Pictures|*.jpg;*.jpeg;*.bmp;*.png";
            System.Windows.Forms.DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                    foreach (string fileName in ofd.FileNames)
                    {

                        fileNameLoad = fileName;
                        //Console.WriteLine(fileName); //Writes the file Name to the Console
                        pictureBox.Load(fileName);
                        image = new Bitmap(fileName);
                        currentWorkableImage = image;
                        tempWorkableImage = image;
                        PropertyItem[] propitems = image.PropertyItems;

                        foreach (PropertyItem propitem in propitems)
                        {

                            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                            try
                            {
                                lblTxtCamera.Text = encoding.GetString(propitems[1].Value);
                                lblTxtDate.Text = encoding.GetString(propitems[15].Value);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }


                        }
                        lblTxtHeight.Text = image.Height.ToString();
                        lblTxtWidth.Text = image.Width.ToString();
                    }
                    LoadHistogram(); //Load the histogram for the image
                    enableItems();   //Enable Menu Controls
            }
        }

        private void saveAsClick_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void saveFile()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "image";
            sfd.Filter = "(*.bmp)|*.bmp|(*.jpg)|*.jpg|(*.Png)|*.Png";
            ImageFormat format = ImageFormat.Png;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string ext = System.IO.Path.GetExtension(sfd.FileName);
                switch (ext)
                {
                    case ".jpg":
                        format = ImageFormat.Jpeg;
                        break;
                    case ".bmp":
                        format = ImageFormat.Bmp;
                        break;
                    case ".Png":
                        format = ImageFormat.Png;
                        break;
                }
                pictureBox.Image.Save(sfd.FileName, format);
            }
        }

        private void exitClick_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

       
        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadHistogram();
        }

        private void LoadHistogram()
        {
            histogramBox.Image = imageBox.importHistogram(currentWorkableImage);
        }

        private void lblBrightness_Click(object sender, EventArgs e)
        {

        }

        private void trackBarBrightness_Scroll(object sender, EventArgs e)
        {
            pushToUndoStack(currentWorkableImage);
            trackBarBrightness.UseWaitCursor = true;
            int BrightnessChange = trackBarBrightness.Value - currentBrightnessLevel;
            currentBrightnessLevel = trackBarBrightness.Value;
            currentWorkableImage = imageBox.changeBrightness(currentWorkableImage, BrightnessChange);
            pictureBox.Image = currentWorkableImage;
            LoadHistogram();
            trackBarBrightness.UseWaitCursor = false;      
        }

        private void trackBarBrightness_ValueChanged(object sender, EventArgs e)
        {

        }

        private void undoItem_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 0)
            {
                resetEditor();
                pushToRedoStack(currentWorkableImage);
                currentWorkableImage = undoStack.Pop();
                pictureBox.Image = currentWorkableImage;
                LoadHistogram();
                if (undoStack.Count == 0)
                {
                    undoItem.Enabled = false;
                }
            }
            else
            {
                undoItem.Enabled = false;
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                pushToUndoStack(currentWorkableImage);
                currentWorkableImage = redoStack.Pop();
                pictureBox.Image = currentWorkableImage;
                LoadHistogram();
                if (redoStack.Count == 0)
                {
                    redoItem.Enabled = false;
                }
            }
            else
            {
                redoItem.Enabled = false;
            }
        }


        private void trackBarContrast_Scroll(object sender, EventArgs e)
        {
            pushToUndoStack(currentWorkableImage);
            trackBarBrightness.UseWaitCursor = true;
            currentContrastLevel = trackBarContrast.Value;
            currentWorkableImage = imageBox.contrastChange(image, currentContrastLevel);
            pictureBox.Image = currentWorkableImage;
            LoadHistogram();
            trackBarBrightness.UseWaitCursor = false; 
        }

        private void verticalFlipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pushToUndoStack(currentWorkableImage);
            currentWorkableImage = imageBox.verticalFlip(currentWorkableImage);
            pictureBox.Image = currentWorkableImage;
        }

        private void pushToUndoStack(Bitmap bitmap)
        {
            if (undoStack.Count <= 20)   //Check for the maximum limit. Otherwise everything will be stored.
            {
                Bitmap image = new Bitmap(bitmap, bitmap.Size);
                undoStack.Push(image);
                undoItem.Enabled = true;
            }
        }

        private void pushToRedoStack(Bitmap bitmap)
        {
            if (redoStack.Count <= 20)   //Check for the maximum limit. Otherwise everything will be stored.
            {
                Bitmap image = new Bitmap(bitmap, bitmap.Size);
                redoStack.Push(image);
                redoItem.Enabled = true;
            }

        }

        private void horizontalFlipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pushToUndoStack(currentWorkableImage);
            currentWorkableImage = imageBox.horizontalFlip(currentWorkableImage);
            pictureBox.Image = currentWorkableImage;
        }

        private void clockwiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pushToUndoStack(currentWorkableImage);
            currentWorkableImage = imageBox.turnClockWise(currentWorkableImage);
            pictureBox.Image = currentWorkableImage;
        }

        private void aniTClockWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pushToUndoStack(currentWorkableImage);
            currentWorkableImage = imageBox.turnCounterClockWise(currentWorkableImage);
            pictureBox.Image = currentWorkableImage;
        }

        private void enableItems()
        {
            grayScaleMenuItem.Enabled = true;
            invertColorsMenuItem.Enabled = true;
            horizontalFlipMenuItem.Enabled = true;
            verticalFlipMenuItem.Enabled = true;
            rotateMenuItem.Enabled = true;
            histogramMenuItem.Enabled = true;
            tabEditorControl.Enabled = true;
            equalizeHistogramMenuItem.Enabled = true;
            normalizeMenuItem.Enabled = true;
            saveAsClick.Enabled = true;
            closeMenuItem.Enabled = true;
            brightnessAdjustmentMenuItem.Enabled = true;
            scaleMenuItem.Enabled = true;
            contrastStretchMenuItem.Enabled = true;
            bitPlaneMenuItem.Enabled = true;
            imageSelectionMenuItem.Enabled = true;
            meanFilterMenuItem.Enabled = true;
            lOGFilterMenuItem.Enabled = true;
            userFilterMenuItem.Enabled = true;
            huffmanMenuItem.Enabled = true;
            //saveClick.Enabled = true;                  save click disabled for safety
        }

        private void disableItems()
        {
            grayScaleMenuItem.Enabled = false;
            invertColorsMenuItem.Enabled = false;
            horizontalFlipMenuItem.Enabled = false;
            verticalFlipMenuItem.Enabled = false;
            rotateMenuItem.Enabled = false;
            histogramMenuItem.Enabled = false;
            tabEditorControl.Enabled = false;
            equalizeHistogramMenuItem.Enabled = false;
            normalizeMenuItem.Enabled = false;
            saveAsClick.Enabled = false;
            closeMenuItem.Enabled = false;
            saveClick.Enabled = false;
            contrastStretchMenuItem.Enabled = false;
            bitPlaneMenuItem.Enabled = false;
            imageSelectionMenuItem.Enabled = false;
            meanFilterMenuItem.Enabled = false;
            lOGFilterMenuItem.Enabled = false;
            userFilterMenuItem.Enabled = false;
            huffmanMenuItem.Enabled = false;
        }
        private void fIlTERSToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void equalizeHistogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pushToUndoStack(currentWorkableImage);
            currentWorkableImage = imageBox.equalizeHistogram(currentWorkableImage);
            pictureBox.Image = currentWorkableImage;
            LoadHistogram();
        }

        private void grayScaleMenuItem_Click(object sender, EventArgs e)
        {
            pushToUndoStack(currentWorkableImage);
            Bitmap img = imageBox.rgbToGray(currentWorkableImage);
            pictureBox.Image = img;
            currentWorkableImage = img;
            LoadHistogram(); 

        }

        private void invertColorsMenuItem_Click(object sender, EventArgs e)
        {
            pushToUndoStack(currentWorkableImage);
            currentWorkableImage = imageBox.invertColors(currentWorkableImage);
            pictureBox.Image = currentWorkableImage;
            LoadHistogram();
        }

        private void normalizeMenuItem_Click(object sender, EventArgs e)
        {
            pushToUndoStack(currentWorkableImage);
            currentWorkableImage = imageBox.normalizeHistogram(currentWorkableImage);
            pictureBox.Image = currentWorkableImage;
            LoadHistogram();
        }

        private void closeMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox.Image = null;
            histogramBox.Image = null;
            currentWorkableImage = null;
            disableItems();
        }

        private void saveClick_Click(object sender, EventArgs e)
        {
            String message = "Are you sure you want to overwrite the image?";
            String caption = "View Editor";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
               pictureBox.Image.Save(fileNameLoad, ImageFormat.Png);
            }
            else if (result == System.Windows.Forms.DialogResult.No)
            {
                this.Close();
            }
        }

        private void brightnessAdjustmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabEditorControl.SelectedIndex = 1;
        }

        private void scaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabEditorControl.SelectedIndex = 2;
        }

        private void comboBoxScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            int number = comboBoxScale.SelectedIndex;
            int method = comboBoxMethod.SelectedIndex;
            Bitmap bitmap = new Bitmap(currentWorkableImage);
            pushToUndoStack(currentWorkableImage);
            comboBoxScale.UseWaitCursor = true;
            switch (number)
            {
                case 0:
                    bitmap = method == 0 ? imageBox.scaleNearestNeighbour(bitmap, 2, 3) : bitmap;
                    bitmap = method == 1 ? imageBox.biLinear(bitmap, 2, 3) : bitmap;
                    pictureBox.Image = bitmap;
                    LoadHistogram();
                    break;
                case 1:
                    bitmap = method == 0 ? imageBox.scaleNearestNeighbour(bitmap, 2, 4) : bitmap;
                    bitmap = method == 1 ? imageBox.biLinear(bitmap, 2, 4) : bitmap;
                    pictureBox.Image = bitmap;
                    LoadHistogram();
                    break;
                case 2:
                    bitmap = method == 0 ? imageBox.scaleNearestNeighbour(bitmap, 2, 5) : bitmap;
                    bitmap = method == 1 ? imageBox.biLinear(bitmap, 2, 5) : bitmap;
                    pictureBox.Image = bitmap;
                    LoadHistogram();
                    break;
                case 3:
                    ScaleBox scalebox = new ScaleBox(this);
                    this.Enabled = false;
                    scalebox.Show();
                    break;

                default:
                    break;
            }
            tempWorkableImage = bitmap;
            currentWorkableImage = tempWorkableImage;
            comboBoxScale.UseWaitCursor=false;
            
        }

        public void customScale()
        {
            this.Enabled = true;
            int number = comboBoxScale.SelectedIndex;
            int method = comboBoxMethod.SelectedIndex;
            Bitmap bitmap = new Bitmap(currentWorkableImage);
            pushToUndoStack(currentWorkableImage);

            bitmap = method == 0 ? imageBox.scaleNearestNeighbour(bitmap, x_scale, y_scale) : bitmap;
            bitmap = method == 1 ? imageBox.biLinear(bitmap, x_scale, y_scale) : bitmap;
            pictureBox.Image = bitmap;
            LoadHistogram();

        }

        private void comboBoxMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxScale.Enabled = true;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox_DragEnter(object sender, DragEventArgs e)
        {
          
        }

        private void pictureBox_DragDrop(object sender, DragEventArgs e)
        {
          
        }

        private void ViewEditor_DragDrop(object sender, DragEventArgs e)
        {
            int x = this.PointToClient(new Point(e.X, e.Y)).X;

            int y = this.PointToClient(new Point(e.X, e.Y)).Y;

            if (x >= pictureBox.Location.X && x <= pictureBox.Location.X + pictureBox.Width && y >= pictureBox.Location.Y && y <= pictureBox.Location.Y + pictureBox.Height)
            {

                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                pictureBox.Image = Image.FromFile(files[0]);
                image = new Bitmap(Image.FromFile(files[0]));
                currentWorkableImage = image;
                tempWorkableImage = image;
                LoadHistogram();
                enableItems();

                PropertyItem[] propitems = image.PropertyItems;

                foreach (PropertyItem propitem in propitems)
                {

                    System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                    try
                    {
                        lblTxtCamera.Text = encoding.GetString(propitems[1].Value);
                        lblTxtDate.Text = encoding.GetString(propitems[15].Value);
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine(err);
                    }


                }
                lblTxtHeight.Text = image.Height.ToString();
                lblTxtWidth.Text = image.Width.ToString();

            }

        }

        private void ViewEditor_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void contrastStretchMenuItem_Click(object sender, EventArgs e)
        {
            tabEditorControl.SelectedIndex = 3;
        }

        private void contrastChange()
        {
            pushToUndoStack(currentWorkableImage);
            currentWorkableImage = imageBox.contrastStretch(image, Convert.ToDouble(alphaValue.Text), Convert.ToDouble(betaValue.Text), Convert.ToDouble(gammaValue.Text), Convert.ToInt32(aValue.Text), Convert.ToInt32(bValue.Text));
            pictureBox.Image = currentWorkableImage;
            LoadHistogram();
        }

        private void alphaValue_ValueChanged(object sender, EventArgs e)
        {
            contrastChange();
        }

        private void betaValue_ValueChanged(object sender, EventArgs e)
        {
            contrastChange();
        }

        private void gammaValue_ValueChanged(object sender, EventArgs e)
        {
            contrastChange();
        }

        private void aValue_ValueChanged(object sender, EventArgs e)
        {
            contrastChange();
            bValue.Enabled = true;
            bValue.Minimum = Convert.ToInt32(aValue.Text);
        }

        private void bValue_ValueChanged(object sender, EventArgs e)
        {
            contrastChange();
        }

        private void plane0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pushToUndoStack(currentWorkableImage);
            currentWorkableImage=imageBox.bitSlice(tempWorkableImage, 0);
            pictureBox.Image = currentWorkableImage;
            LoadHistogram();
        }

        private void plane1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pushToUndoStack(currentWorkableImage);
            currentWorkableImage = imageBox.bitSlice(tempWorkableImage, 1);
            pictureBox.Image = currentWorkableImage;
            LoadHistogram();

        }

        private void plane7MenuItem_Click(object sender, EventArgs e)
        {
            pushToUndoStack(currentWorkableImage);
            currentWorkableImage = imageBox.bitSlice(tempWorkableImage, 7);
            pictureBox.Image = currentWorkableImage;
            LoadHistogram();

        }

        private void plane6MenuItem_Click(object sender, EventArgs e)
        {
            pushToUndoStack(currentWorkableImage);
            currentWorkableImage = imageBox.bitSlice(tempWorkableImage, 6);
            pictureBox.Image = currentWorkableImage;
            LoadHistogram();
        }

        private void plane5MenuItem_Click(object sender, EventArgs e)
        {
            pushToUndoStack(currentWorkableImage);
            currentWorkableImage = imageBox.bitSlice(tempWorkableImage, 5);
            pictureBox.Image = currentWorkableImage;
            LoadHistogram();
        }

        private void plane4MenuItem_Click(object sender, EventArgs e)
        {
            pushToUndoStack(currentWorkableImage);
            currentWorkableImage = imageBox.bitSlice(tempWorkableImage, 4);
            pictureBox.Image = currentWorkableImage;
            LoadHistogram();
        }

        private void plane3MenuItem_Click(object sender, EventArgs e)
        {
            pushToUndoStack(currentWorkableImage);
            currentWorkableImage = imageBox.bitSlice(tempWorkableImage, 3);
            pictureBox.Image = currentWorkableImage;
            LoadHistogram();
        }

        private void plane2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pushToUndoStack(currentWorkableImage);
            currentWorkableImage = imageBox.bitSlice(tempWorkableImage, 2);
            pictureBox.Image = currentWorkableImage;
            LoadHistogram();
        }

        private void currentImageMenuItem_Click(object sender, EventArgs e)
        {
            originalImageMenuItem.Checked = false;
            tempWorkableImage = currentWorkableImage;
        }

        private void originalImageMenuItem_Click(object sender, EventArgs e)
        {
            currentImageMenuItem.Checked = false;
            tempWorkableImage = image;
        }

        private void meanFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
        
        }

        private void x5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pushToUndoStack(currentWorkableImage);
            currentWorkableImage = imageBox.meanFilter(currentWorkableImage, 5);
            pictureBox.Image = currentWorkableImage;
            LoadHistogram();
        }

        private void x7ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pushToUndoStack(currentWorkableImage);
            currentWorkableImage = imageBox.meanFilter(currentWorkableImage, 7);
            pictureBox.Image = currentWorkableImage;
            LoadHistogram();
        }

        private void x3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pushToUndoStack(currentWorkableImage);
            currentWorkableImage = imageBox.meanFilter(currentWorkableImage, 3);
            pictureBox.Image = currentWorkableImage;
            LoadHistogram();
        }

        /** log filter */
        private void x5ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            pushToUndoStack(currentWorkableImage);
            currentWorkableImage = imageBox.meanFilter(currentWorkableImage, 5);
            currentWorkableImage = imageBox.logFilter(currentWorkableImage, 5);
            pictureBox.Image = currentWorkableImage;
            LoadHistogram();

        }

        private void x7ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            pushToUndoStack(currentWorkableImage);
            currentWorkableImage = imageBox.meanFilter(currentWorkableImage, 7);
            currentWorkableImage = imageBox.logFilter(currentWorkableImage, 7);
            pictureBox.Image = currentWorkableImage;
            LoadHistogram();
        }

        private void ViewEditor_Load(object sender, EventArgs e)
        {

        }

        private void userFilterMenuItem_Click(object sender, EventArgs e)
        {
            UserMask userMask = new UserMask(this);
            this.Enabled = false;
            userMask.Show();
        }

        public void userFilterCall(double[,] mask)
        {
            this.Enabled = true;
            pushToUndoStack(currentWorkableImage);
            currentWorkableImage = imageBox.userFilter(currentWorkableImage, 3, mask);
            pictureBox.Image = currentWorkableImage;
            LoadHistogram();

        }

        private void huffmannToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageBox.HuffmanEncode(currentWorkableImage);
            String message = "Huffman Code Generated: Check Folder";
            String caption = "View Editor";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result = MessageBox.Show(message, caption, buttons);
            System.Diagnostics.Process.Start("Encoded.txt");

        }

        private void resetEditor()
        {
            trackBarBrightness.Value = 0;
            trackBarContrast.Value = 0;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Visible = true;
        }



 

       
        
    }
}
