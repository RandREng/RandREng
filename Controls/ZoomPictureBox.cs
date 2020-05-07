using System;
using System.Drawing;
using System.Windows.Forms;

namespace RandREng.Controls
{
    public partial class ZoomPictureBox : UserControl
	{
		public float Zoom { get; set; }

		private Image _image;
		public Image Image
		{
			get
			{
				return _image;
			}
			set
			{
				this._image = value;
				this.offset.X = 0;
				this.offset.Y = 0;
				this.lastPaint.X = 0;
				this.lastPaint.Y = 0;
				if (value != null)
				{
					Zoom = ((float)this.Image.Width) / this.ClientRectangle.Width;
				}
				this.mouseDown = false;
				this.Refresh();
			}
		}

		private Point offset;

		private Point mouse;
		private bool mouseDown { get; set; }
		private Point initXY;
		private Point lastPaint;

		public ZoomPictureBox()
		{
			InitializeComponent();
			Zoom = 1.00f;
			this.offset.X = 0;
			this.offset.Y = 0;
			this.lastPaint.X = 0;
			this.lastPaint.Y = 0;
			this.mouseDown = false;
			this.DoubleBuffered = true;
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			Zoom *= (1.0f + (e.Delta / 1200.0f));
			System.Console.WriteLine(string.Format("{0} - {1}", e.Delta, Zoom));
			this.Invalidate();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (this.mouseDown && e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				offset.X = (int) (initXY.X + (mouse.X - e.X) * Zoom);
				offset.Y = (int) (initXY.Y + (mouse.Y - e.Y) * Zoom);
				if (Math.Abs(offset.X - lastPaint.X) > 5 || Math.Abs(offset.Y - lastPaint.Y) > 5)
				{
					this.Invalidate();
				}
			}
			base.OnMouseMove(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			this.mouseDown = false;
			this.Invalidate();
			base.OnMouseUp(e);
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				this.mouseDown = true;
				this.mouse.X = e.X;
				this.mouse.Y = e.Y;
				this.initXY.X = offset.X;
				this.initXY.Y = offset.Y;
			}
			else if (e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				if (this.Image != null)
				{
					this.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
				}
			}
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			if (this.Image != null)
			{
				this.lastPaint.X = offset.X;
				this.lastPaint.Y = offset.Y;

				e.Graphics.DrawImage(this.Image, this.ClientRectangle, offset.X, offset.Y, this.ClientRectangle.Width * Zoom, this.ClientRectangle.Height * Zoom, GraphicsUnit.Pixel);
			}
			else
			{
				base.OnPaint(e);
			}
		}

		private void ZoomPictureBox_ClientSizeChanged(object sender, EventArgs e)
		{
			if (this.Image != null)
			{
				Zoom = ((float)this.Image.Width) / this.ClientRectangle.Width;
			}
			this.mouseDown = false;
			this.Refresh();

		}
	}
}
