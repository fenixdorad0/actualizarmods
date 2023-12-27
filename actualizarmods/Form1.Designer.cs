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
            label3 = new Label();
            buttonDescargarFtp = new Button();
            labelState = new Label();
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 9);
            label3.Name = "label3";
            label3.Size = new Size(251, 15);
            label3.TabIndex = 3;
            label3.Text = "Recuerda abrir el programa como admistrador";
            // 
            // buttonDescargarFtp
            // 
            buttonDescargarFtp.Location = new Point(12, 95);
            buttonDescargarFtp.Name = "buttonDescargarFtp";
            buttonDescargarFtp.Size = new Size(137, 23);
            buttonDescargarFtp.TabIndex = 9;
            buttonDescargarFtp.Text = "Descargar Mods";
            buttonDescargarFtp.UseVisualStyleBackColor = true;
            buttonDescargarFtp.Click += button2_Click_1;
            // 
            // labelState
            // 
            labelState.AutoSize = true;
            labelState.Location = new Point(12, 121);
            labelState.Name = "labelState";
            labelState.Size = new Size(120, 15);
            labelState.TabIndex = 10;
            labelState.Text = "Estado de la descarga";
            // 
            // button1
            // 
            button1.Location = new Point(12, 66);
            button1.Name = "button1";
            button1.Size = new Size(137, 23);
            button1.TabIndex = 11;
            button1.Text = "Instalar forge";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(12, 37);
            button2.Name = "button2";
            button2.Size = new Size(137, 23);
            button2.TabIndex = 12;
            button2.Text = "Instalar Java";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 148);
            label1.Name = "label1";
            label1.Size = new Size(170, 15);
            label1.TabIndex = 13;
            label1.Text = "Galaxy Worlds IP: 186.31.27.110";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(441, 280);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(labelState);
            Controls.Add(buttonDescargarFtp);
            Controls.Add(label3);
            Name = "Form1";
            Text = "Launcher por Fenixdorad0";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label3;
        private Button buttonDescargarFtp;
        private Label labelState;
        private Button button1;
        private Button button2;
        private Label label1;
    }
}
