using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace Webcam
{
    public partial class Form1 : Form
    {

        private bool DeviceExist = false;
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource = null;


        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap img = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = img;
        }


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                comboBox1.Items.Clear();
                if (videoDevices.Count == 0)
                    throw new ApplicationException();

                DeviceExist = true;
                foreach (FilterInfo device in videoDevices)
                {
                    comboBox1.Items.Add(device.Name);
                }
                comboBox1.SelectedIndex = 0;
            }
            catch (ApplicationException)
            {
                DeviceExist = false;
                comboBox1.Items.Add("Nenhum dispositivo encontrado!");
            }
        }

        private void btnCapturar_Click(object sender, EventArgs e)
        {
            if (btnCapturar.Text == "Capturar" || btnCapturar.Text == "Capturar outra Foto")
            {
                if (DeviceExist)
                {
                    videoSource = new VideoCaptureDevice(videoDevices[comboBox1.SelectedIndex].MonikerString);
                    videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);

                    //Encerra o sinal da camera.
                    if (!(videoSource == null))
                        if (videoSource.IsRunning)
                        {
                            videoSource.SignalToStop();
                            videoSource = null;
                        }
                    videoSource.DesiredFrameSize = new Size(160, 120);
                    videoSource.DesiredFrameRate = 10;
                    videoSource.Start();

                    btnCapturar.Text = "Capturar Foto";
                }
                else
                {
                    MessageBox.Show("Nenhum dispositivo encontrado!");
                }
            }
            else
            {
                if (videoSource.IsRunning)
                {
                    //Encerra o sinal da camera.
                    // Note que este bloco de código se repetiu, você pode cirar um método para ele.
                    if (!(videoSource == null))
                        if (videoSource.IsRunning)
                        {
                            videoSource.SignalToStop();
                            videoSource = null;

                            // salva a imagem;
                            saveFileDialog1.Filter = "JPEG (*.jpg;*.jpeg;*jpeg;*.jfif)|*.jpg;*.jpeg;*jpeg;*.jfif";
                            DialogResult res = saveFileDialog1.ShowDialog();
                            if (res == DialogResult.OK)
                            {
                                pictureBox1.Image.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);

                                btnCapturar.Text = "Capturar outra Foto";
                            }
                        }
                }
            }
        }

    }
}
    
 
    


