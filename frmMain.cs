using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FolderSizesChart
{
    public partial class frmMain : Form
    {
        string mstrRootFolder = "";
        private Boolean mblnDoneOnce = false;
        private Bitmap mobjFormBitmap;
        private Graphics mobjBitmapGraphics;
        private int mintFormWidth;
        private int mintFormHeight;
        private double mdblScale;
        private bool mblnProcessingTopFolder;
        private List<FolderPlotInfo> mlstFoldersPlot;
        private bool mblnFoldersInfoBuilt = false;
        private int mintMaxY = 0;
        private int mintMaxX1 = 0;
        private Dictionary<string, string> mdctExpandedList;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            FolderBrowserDialog objFolderBroweserDialog;

            objFolderBroweserDialog = new FolderBrowserDialog();
            objFolderBroweserDialog.Description = "Select root folder";
            DialogResult objResult = objFolderBroweserDialog.ShowDialog();

            if (objResult == DialogResult.OK)
            {
                mstrRootFolder = objFolderBroweserDialog.SelectedPath;
                this.WindowState = FormWindowState.Minimized;
                this.WindowState = FormWindowState.Normal;
            }
            else
                Application.Exit();        
        }

        private void frmMain_Activated(object sender, EventArgs e)
        {
            if (!mblnDoneOnce)
            {
                mblnDoneOnce = true;
                mintFormWidth = this.Width;
                mintFormHeight = this.Height;
                mobjFormBitmap = new Bitmap(mintFormWidth, mintFormHeight, this.CreateGraphics());
                mobjBitmapGraphics = Graphics.FromImage(mobjFormBitmap);

                mdblScale = 0.000001;
            }
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                mintFormWidth = this.Width;
                mintFormHeight = this.Height;
                mobjFormBitmap = new Bitmap(mintFormWidth, mintFormHeight, this.CreateGraphics());
                mobjBitmapGraphics = Graphics.FromImage(mobjFormBitmap);
                RefreshDisplay();
            }
        }

        private void frmMain_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (scrScroll.Value - scrScroll.SmallChange > 0)
                {
                    scrScroll.Value -= scrScroll.SmallChange;
                }
                else
                {
                    scrScroll.Value = 0;
                }
            }
            else
            {
                if (scrScroll.Value + scrScroll.SmallChange < scrScroll.Maximum - scrScroll.LargeChange + 1)
                {
                    scrScroll.Value += scrScroll.SmallChange;
                }
                else
                {
                    scrScroll.Value = scrScroll.Maximum - scrScroll.LargeChange + 1;
                }
            }
            RefreshDisplay();
        }

        private void scrScroll_Scroll(object sender, ScrollEventArgs e)
        {
            RefreshDisplay();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //Do nothing
        }

        private void frmMain_Paint(object sender, PaintEventArgs e)
        {
            if (mobjFormBitmap != null)
                e.Graphics.DrawImage(mobjFormBitmap, 0, 0);
        }

        private void RefreshDisplay()
        {
            Font objFont;
            int intYShift;
            mobjBitmapGraphics.FillRectangle(Brushes.White, 0, 0, mintFormWidth, mintFormHeight);

            if (mstrRootFolder != "")        // if user selected a folder
            {
                if (!mblnFoldersInfoBuilt)
                {
                    mblnFoldersInfoBuilt = true;
                    int intY = 10;
                    mlstFoldersPlot = new List<FolderPlotInfo>();
                    mdctExpandedList = new Dictionary<string, string>();
                    mdctExpandedList.Add(mstrRootFolder, "x");
                    mintMaxY = 0;
                    mintMaxX1 = 0;
                    mblnProcessingTopFolder = true;
                    BuildDirectorySizesList(mstrRootFolder, 10,ref intY);
                    scrScroll.Minimum = 0;
                    scrScroll.SmallChange = 35;
                    scrScroll.LargeChange = mintFormHeight;
                    scrScroll.Maximum = mintMaxY + mintFormHeight - 36;
                }

                int intXBar = mintMaxX1;
                while (intXBar >= 10)
                {
                    mobjBitmapGraphics.DrawLine(Pens.LightBlue,intXBar, 8, intXBar, mintFormHeight);
                    intXBar -= 15;
                }

                intYShift = scrScroll.Value;
                foreach (FolderPlotInfo objFolderPlotInfo in mlstFoldersPlot)
                {
                    objFont = new Font("MS Sans Serif", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
                    if (!objFolderPlotInfo.ErrorReading)
                    {
                        // white background for the folder name
                        mobjBitmapGraphics.FillRectangle(Brushes.White, objFolderPlotInfo.X1, objFolderPlotInfo.Y1 - 17 - intYShift, mintFormWidth - objFolderPlotInfo.X1, 17);
                        mobjBitmapGraphics.DrawString(objFolderPlotInfo.DisplayPath, objFont, Brushes.Black, objFolderPlotInfo.X1, objFolderPlotInfo.Y1 - 15 - intYShift);

                        mobjBitmapGraphics.FillRectangle(Brushes.Blue, objFolderPlotInfo.X1, objFolderPlotInfo.Y1 - intYShift, Convert.ToInt32((double)objFolderPlotInfo.TotalSize * mdblScale), objFolderPlotInfo.Y2 - objFolderPlotInfo.Y1);
                        mobjBitmapGraphics.FillRectangle(Brushes.Red, objFolderPlotInfo.X1, objFolderPlotInfo.Y1 - intYShift, Convert.ToInt32((double)objFolderPlotInfo.TopFolderSize * mdblScale), objFolderPlotInfo.Y2 - objFolderPlotInfo.Y1);

                        // white background for the folder size text
                        mobjBitmapGraphics.FillRectangle(Brushes.White, objFolderPlotInfo.X1 + Convert.ToInt32((double)objFolderPlotInfo.TotalSize * mdblScale), objFolderPlotInfo.Y1 - intYShift, mintFormWidth - objFolderPlotInfo.X1 - Convert.ToInt32((double)objFolderPlotInfo.TotalSize * mdblScale), objFolderPlotInfo.Y2 - objFolderPlotInfo.Y1);                        
                        mobjBitmapGraphics.DrawString(GetSizeName(objFolderPlotInfo.TotalSize), objFont, Brushes.Black, objFolderPlotInfo.X1 + Convert.ToInt32((double)objFolderPlotInfo.TotalSize * mdblScale) + 5, objFolderPlotInfo.Y1 - intYShift);
                    }
                    else
                    {
                        // white background for the text
                        mobjBitmapGraphics.FillRectangle(Brushes.White, objFolderPlotInfo.X1, objFolderPlotInfo.Y1 - 17 - intYShift, mintFormWidth - objFolderPlotInfo.X1, 17);

                        mobjBitmapGraphics.DrawString(objFolderPlotInfo.DisplayPath + " (error reading from this folder)", objFont, Brushes.Black, objFolderPlotInfo.X1, objFolderPlotInfo.Y1 - 15 - intYShift);
                    }                    
                }
            }

            this.Invalidate();
        }


        private long BuildDirectorySizesList(string pstrFolderPath,int pintX,ref int pintY)
        {
            long lngTotalSize = 0;
            long lngTopFolderSize;
            int intSaveY;
            bool blnIsTopFolder = false;

            blnIsTopFolder = mblnProcessingTopFolder;
            mblnProcessingTopFolder = false;
            intSaveY = pintY;
            try
            {
                foreach (string strFileName in Directory.GetFiles(pstrFolderPath, "*.*", SearchOption.TopDirectoryOnly))
                {
                    FileInfo info = new FileInfo(strFileName);
                    lngTotalSize += info.Length;
                }

                lngTopFolderSize = lngTotalSize;

                pintY += 35;
                foreach (string strFolderName in Directory.GetDirectories(pstrFolderPath, "*.*", SearchOption.TopDirectoryOnly))
                {
                    if (mdctExpandedList.ContainsKey(pstrFolderPath))
                        lngTotalSize += BuildDirectorySizesList(strFolderName, pintX + 15, ref pintY);
                    else
                        lngTotalSize += GetDirectorySize(strFolderName);
                    Application.DoEvents();
                }

                FolderPlotInfo objFolderPlotInfo = new FolderPlotInfo();

                objFolderPlotInfo.FullPath = pstrFolderPath;
                if (blnIsTopFolder)
                    objFolderPlotInfo.DisplayPath = pstrFolderPath; 
                else
                    objFolderPlotInfo.DisplayPath = Path.GetFileName(pstrFolderPath);

                objFolderPlotInfo.X1 = pintX;
                objFolderPlotInfo.Y1 = intSaveY + 15;
                objFolderPlotInfo.X2 = pintX + Convert.ToInt32((double)lngTotalSize * mdblScale);
                objFolderPlotInfo.Y2 = intSaveY + 15 + 18;
                objFolderPlotInfo.TotalSize = lngTotalSize;
                objFolderPlotInfo.TopFolderSize = lngTopFolderSize;

                if (mintMaxY < objFolderPlotInfo.Y2)
                    mintMaxY = objFolderPlotInfo.Y2;

                if (mintMaxX1 < objFolderPlotInfo.X1)
                    mintMaxX1 = objFolderPlotInfo.X1;

                mlstFoldersPlot.Add(objFolderPlotInfo);

                return lngTotalSize;
            }
            catch
            {
                FolderPlotInfo objFolderPlotInfo = new FolderPlotInfo();
                
                pintY += 17;

                objFolderPlotInfo.FullPath = pstrFolderPath;
                if (blnIsTopFolder)
                    objFolderPlotInfo.DisplayPath = pstrFolderPath;
                else
                    objFolderPlotInfo.DisplayPath = Path.GetFileName(pstrFolderPath);
                objFolderPlotInfo.ErrorReading = true;
                objFolderPlotInfo.X1 = pintX;
                objFolderPlotInfo.Y1 = intSaveY + 15;

                mlstFoldersPlot.Add(objFolderPlotInfo);

                return 0;
            }
        }


        private long GetDirectorySize(string pstrFolderPath)
        {
            long lngTotalSize = 0;

            try
            {
                foreach (string strFileName in Directory.GetFiles(pstrFolderPath, "*.*", SearchOption.TopDirectoryOnly))
                {
                    FileInfo info = new FileInfo(strFileName);
                    lngTotalSize += info.Length;
                }

                foreach (string strFolderName in Directory.GetDirectories(pstrFolderPath, "*.*", SearchOption.TopDirectoryOnly))
                {
                    lngTotalSize += GetDirectorySize(strFolderName);
                    Application.DoEvents();
                }
                return lngTotalSize;
            }
            catch
            {
                return 0;
            }
        }

        private string GetSizeName(long plngSize)
        {
            double dblSize = plngSize;
            string strSize;

            if (dblSize < 1024)
            {
                return plngSize.ToString() + " bytes";
            }
            else
            {
                dblSize /= 1024;
                if (dblSize < 1024)
                {
                    strSize = string.Format("{0:#.##}",dblSize);
                    if (strSize.EndsWith("."))
                    {
                        strSize = strSize.Substring(0,strSize.Length - 1);
                    }
                    return strSize + " KB";
                }
                else
                {
                    dblSize /= 1024;
                    if (dblSize < 1024)
                    {
                        strSize = string.Format("{0:#.##}", dblSize);
                        if (strSize.EndsWith("."))
                        {
                            strSize = strSize.Substring(0, strSize.Length - 1);
                        }
                        return strSize + " MB";
                    }
                    else
                    {
                        dblSize /= 1024;
                        strSize = string.Format("{0:#.##}", dblSize);
                        if (strSize.EndsWith("."))
                        {
                            strSize = strSize.Substring(0, strSize.Length - 1);
                        }
                        return strSize + " GB";
                    }
                }
            }
        }

        private void btnScaleDown_Click(object sender, EventArgs e)
        {
            mdblScale /= 2;
            RefreshDisplay();
        }

        private void btnScaleUp_Click(object sender, EventArgs e)
        {
            mdblScale *= 2;
            RefreshDisplay();
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (scrScroll.Value - scrScroll.SmallChange > 0)
                    {
                        scrScroll.Value -= scrScroll.SmallChange;
                    }
                    else
                    {
                        scrScroll.Value = 0;
                    }
                    RefreshDisplay();
                    break;

                case Keys.Down:
                    if (scrScroll.Value + scrScroll.SmallChange < scrScroll.Maximum - scrScroll.LargeChange + 1)
                    {
                        scrScroll.Value += scrScroll.SmallChange;
                    }
                    else
                    {
                        scrScroll.Value = scrScroll.Maximum - scrScroll.LargeChange + 1;
                    }
                    RefreshDisplay();
                    break;

                case Keys.PageUp:
                    if (scrScroll.Value - scrScroll.LargeChange > 0)
                    {
                        scrScroll.Value -= scrScroll.LargeChange;
                    }
                    else
                    {
                        scrScroll.Value = 0;
                    }
                    RefreshDisplay();

                    break;

                case Keys.PageDown:
                    if (scrScroll.Value + scrScroll.LargeChange < scrScroll.Maximum - scrScroll.LargeChange + 1)
                    {
                        scrScroll.Value += scrScroll.LargeChange;
                    }
                    else
                    {
                        scrScroll.Value = scrScroll.Maximum - scrScroll.LargeChange + 1;
                    }
                    RefreshDisplay();
                    break;
            }
        }

        private void frmMain_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (FolderPlotInfo objFolderPlotInfo in mlstFoldersPlot)
            {
                if (e.Y > objFolderPlotInfo.Y1 - 15 - scrScroll.Value && e.Y < objFolderPlotInfo.Y2 - scrScroll.Value) // clicked within this folder boundaries
                {
                    Cursor.Current = Cursors.AppStarting;
                    // toggle the expanded / not expanded status
                    if (mdctExpandedList.ContainsKey(objFolderPlotInfo.FullPath))
                        mdctExpandedList.Remove(objFolderPlotInfo.FullPath);
                    else
                        mdctExpandedList.Add(objFolderPlotInfo.FullPath,"x");

                    int intY = 10;
                    mlstFoldersPlot = new List<FolderPlotInfo>();
                    mintMaxY = 0;
                    mintMaxX1 = 0;
                    mblnProcessingTopFolder = true;
                    BuildDirectorySizesList(mstrRootFolder, 10, ref intY);
                    scrScroll.Maximum = mintMaxY + mintFormHeight - 36;

                    Cursor.Current = Cursors.Default;
                }
            }
            RefreshDisplay();
        }


    }

    public class FolderPlotInfo
    {
        public string FullPath;
        public string DisplayPath;
        public int X1;
        public int Y1;
        public int X2;
        public int Y2;
        public long TotalSize;
        public long TopFolderSize;
        public bool ErrorReading = false;
    }

}
