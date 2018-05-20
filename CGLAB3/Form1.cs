using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CGLAB3
{
    public partial class Form1 : Form
    {
        GLGraphics GLGraphics = new GLGraphics();
        RayTracing rt = new RayTracing();
        public Form1()
        {
            InitializeComponent();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            //GLGraphics.Resize(glControl1.Width, glControl1.Height);
            rt.Resize(glControl1.Width, glControl1.Height);
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            //GLGraphics.Update();
            rt.Update();
            glControl1.SwapBuffers();
            rt.CloseProgram();
        }

        private void Application_Idle(object sender, PaintEventArgs e)
        {

            glControl1_Paint(sender, e);

        }
    }
}
