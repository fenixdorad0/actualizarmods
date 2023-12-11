namespace actualizarmods
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
            buttonDescargar = new Button();
            buttonIniciarServer = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            buttonObtenerIP = new Button();
            SuspendLayout();
            // 
            // buttonDescargar
            // 
            buttonDescargar.Location = new Point(12, 79);
            buttonDescargar.Name = "buttonDescargar";
            buttonDescargar.Size = new Size(75, 23);
            buttonDescargar.TabIndex = 0;
            buttonDescargar.Text = "Descargar";
            buttonDescargar.UseVisualStyleBackColor = true;
            buttonDescargar.Click += button1_Click;
            // 
            // buttonIniciarServer
            // 
            buttonIniciarServer.Location = new Point(93, 79);
            buttonIniciarServer.Name = "buttonIniciarServer";
            buttonIniciarServer.Size = new Size(75, 23);
            buttonIniciarServer.TabIndex = 1;
            buttonIniciarServer.Text = "Iniciar servidor";
            buttonIniciarServer.UseVisualStyleBackColor = true;
            buttonIniciarServer.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 52);
            label1.Name = "label1";
            label1.Size = new Size(101, 15);
            label1.TabIndex = 2;
            label1.Text = "Que esta pasando";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 20);
            label2.Name = "label2";
            label2.Size = new Size(43, 15);
            label2.TabIndex = 2;
            label2.Text = "Eres el:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 125);
            label3.Name = "label3";
            label3.Size = new Size(101, 15);
            label3.TabIndex = 3;
            label3.Text = "Notas de version1";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(271, 30);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(100, 23);
            textBox1.TabIndex = 4;
            textBox1.Text = "192.168.0.200";
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(271, 59);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(100, 23);
            textBox2.TabIndex = 5;
            textBox2.Text = "8080";
            // 
            // buttonObtenerIP
            // 
            buttonObtenerIP.Location = new Point(271, 88);
            buttonObtenerIP.Name = "buttonObtenerIP";
            buttonObtenerIP.Size = new Size(100, 23);
            buttonObtenerIP.TabIndex = 6;
            buttonObtenerIP.Text = "Configurar IP";
            buttonObtenerIP.UseVisualStyleBackColor = true;
            buttonObtenerIP.Click += button3_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(441, 160);
            Controls.Add(buttonObtenerIP);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(buttonIniciarServer);
            Controls.Add(buttonDescargar);
            Name = "Form1";
            Text = "Launcher by fenixdorad0";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonDescargar;
        private Button buttonIniciarServer;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox textBox1;
        private TextBox textBox2;
        private Button buttonObtenerIP;
    }
}
