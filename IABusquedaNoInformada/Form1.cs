using System.Drawing.Imaging;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

//Pendiente Validacion de asignacion punto final genera error al asignar ultimo valor y
namespace IABusquedaNoInformada
{
    public partial class Form1 : Form
    {

        int[,] inicio = { {0,0,0,0,0},
                          {0,0,0,0,0},
                          {0,0,0,0,0},
                          {0,0,0,0,0},
                          {0,0,0,0,0} };
        int time1 = 100;
        int fila, columna = 0;
        int[,] matriz;
        bool flag1;
        //Variables que Guardan el conteo de los caminos
        int cmalos;
        int cbueno;
        int ctope;
        //Listas para Genera Graficado
        List<int> rX = new List<int>();
        List<int> rY = new List<int>();


        // Variables para la busqueda
        int inicioX, inicioY, finX, finY;
        List<int> obsX = new List<int>();
        List<int> obsY = new List<int>();

        //Coordenadas para pintar las celdas
        List<int> rutaX = new List<int>();
        List<int> rutaY = new List<int>();

        //Coordenadas Genericas Prueba
        List<int> rutaX1 = new List<int>();
        List<int> rutaY1 = new List<int>();


        public Form1()
        {
            InitializeComponent();

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            if (matriz == null)
            {
                MessageBox.Show("Defina Primero las dimenciones de la matriz");
            }
            else
            {
                if (comboBox4.Text == "" | comboBox3.Text == "")
                {
                    MessageBox.Show("Se necesita Definir las coordenadas de Destino");
                }
                else
                {
                    //Validacion que no se posicione el punto final en el punto origen

                    if (matriz[Convert.ToInt32(comboBox4.Text), Convert.ToInt32(comboBox3.Text)] == 1)
                    {
                        MessageBox.Show("No puede Definir el Final donde comienza el camino");
                    }
                    else
                    {
                        //Recorrido para Borrar solo los 2 es decir los puntos finales
                        //y definir nuevamente su valor por defecto
                        for (int i = 0; i < matriz.GetLength(0); i++)
                        {
                            for (int j = 0; j < matriz.GetLength(1); j++)
                            {
                                if (matriz[i, j] == 2)
                                {
                                    matriz[i, j] = 0;
                                }
                            }
                        }


                        //realizamos un Recorrido para restaurar primero cada una de las celdas a su color Normal
                        for (int i = 0; i < matriz.GetLength(0); i++)
                        {
                            for (int j = 0; j < matriz.GetLength(1); j++)
                            {
                                dataGridView1.Rows[Convert.ToInt32(i)].Cells[Convert.ToInt32(j)].Style.BackColor = SystemColors.ControlLightLight;
                            }
                        }
                        //Validacion para hacer necesario el ingreso de datos por ComboBox
                        if (comboBox3.Text.Equals("") || comboBox4.Text.Equals(""))
                        {
                            MessageBox.Show("Verifique Seleccion de Fila o Columna.");

                        }
                        else
                        {

                            dataGridView1.Rows[Convert.ToInt32(comboBox4.Text)].Cells[Convert.ToInt32(comboBox3.Text)].Style.BackColor = Color.Red;
                            matriz[Convert.ToInt32(comboBox4.Text), Convert.ToInt32(comboBox3.Text)] = 2;

                        }
                        //Recorrido para Verificar La asignacion de 2 a la Matriz
                        richTextBox1.AppendText("Configuracion Final de la Matriz\n");
                        for (int i = 0; i < matriz.GetLength(0); i++)
                        {
                            for (int j = 0; j < matriz.GetLength(1); j++)
                            {
                                richTextBox1.AppendText(Convert.ToString(matriz[i, j]) + " "); ;
                            }
                            richTextBox1.AppendText("\n");
                        }
                    }
                }
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (matriz == null)
            {
                MessageBox.Show("Defina Primero las dimenciones de la Matriz");
            }
            else
            {
                for (int i = 0; i < matriz.GetLength(0); i++)
                {
                    for (int j = 0; j < matriz.GetLength(1); j++)
                    {
                        dataGridView1.Rows[Convert.ToInt32(i)].Cells[Convert.ToInt32(j)].Style.BackColor = SystemColors.ControlLightLight;
                    }
                }

                for (int i = 0; i < matriz.GetLength(0); i++)
                {
                    for (int j = 0; j < matriz.GetLength(1); j++)
                    {
                        matriz[i, j] = 0;

                    }
                }


                richTextBox1.Clear();
                richTextBox1.AppendText("Se Limpio Tablero, Matriz Restaurada\n\n");

                for (int i = 0; i < matriz.GetLength(0); i++)
                {
                    for (int j = 0; j < matriz.GetLength(1); j++)
                    {
                        richTextBox1.AppendText(Convert.ToString(matriz[i, j]) + " "); ;
                    }
                    richTextBox1.AppendText("\n");
                }

                await Task.Delay(2500);
                richTextBox1.Clear();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Solicitu de las Dimenciones de la Matriz
            fila = Int32.Parse(textBox1.Text);
            columna = Int32.Parse(textBox2.Text);
            // Configuraciones iniciales de la matriz
            dataGridView1.RowCount = fila;
            dataGridView1.ColumnCount = columna;
            dataGridView1.RowHeadersWidth = 50;
            dataGridView1.ColumnHeadersHeight = 10;

            //Cabeceras x e Y de las Matrices
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[i].Name = Convert.ToString(i);
            }
            for (int j = 0; j < dataGridView1.RowCount; j++)
            {
                dataGridView1.Rows[j].HeaderCell.Value = Convert.ToString(j);
            }

            //Creacion de la matriz 
            int[,] matriz = new int[fila, columna];
            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                for (int j = 0; j < matriz.GetLength(1); j++)
                {
                    matriz[i, j] = 0;
                }
            }
            //Recorrido de la Matriz
            /*for (int i = 0; i < matriz.GetLength(0); i++)
            {
                for (int j = 0; j < matriz.GetLength(1); j++)
                {
                    richTextBox1.AppendText(Convert.ToString(matriz[i, j]) + " "); ;
                }
                richTextBox1.AppendText("\n");
            }*/

            //Limpiamos los listados de previos Items
            comboBox4.Items.Clear();
            comboBox3.Items.Clear();

            // Agregamos Items a nuestro ComboBox Filas
            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                comboBox4.Items.Add(i);
            }

            //Agregamos Items al ComboBox Columnas

            for (int j = 0; j < matriz.GetLength(1); j++)
            {
                comboBox3.Items.Add(j);
            }




        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            richTextBox1.Clear();
            matriz = new int[fila, columna];


            richTextBox1.AppendText("Inicio en: " + dataGridView1.CurrentRow.Index + "," + dataGridView1.CurrentCell.ColumnIndex + "\n");
            label22.Text = dataGridView1.CurrentRow.Index.ToString();
            label23.Text = dataGridView1.CurrentCell.ColumnIndex.ToString();
            matriz[dataGridView1.CurrentRow.Index, dataGridView1.CurrentCell.ColumnIndex] = 1;

            richTextBox1.AppendText("\nEstado de La Matriz: \n");
            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                for (int j = 0; j < matriz.GetLength(1); j++)
                {
                    richTextBox1.AppendText(Convert.ToString(matriz[i, j]) + " "); ;
                }
                richTextBox1.AppendText("\n");
            }




        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (matriz == null)
            {
                MessageBox.Show("Defina Primero las dimenciones de la MAtriz");
            }
            else
            {
                richTextBox1.Clear();
                richTextBox1.AppendText("Matriz Final: \n");
                for (int i = 0; i < matriz.GetLength(0); i++)
                {
                    for (int j = 0; j < matriz.GetLength(1); j++)
                    {
                        richTextBox1.AppendText(Convert.ToString(matriz[i, j]) + " "); ;
                    }
                    richTextBox1.AppendText("\n");
                }
            }


        }
        //Agregamos-------------------------------------------------------- Los OBSTACULOS
        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (matriz == null)
            {
                MessageBox.Show("Es necesario Definir Primero las Dimenciones de la MATRIZ :)");
            }
            else
            {
                richTextBox1.Clear();
                //Se define en la matriz el numero 3 como representacion de Obstaculo
                matriz[dataGridView1.CurrentRow.Index, dataGridView1.CurrentCell.ColumnIndex] = 3;

                richTextBox1.AppendText("Obstaculo Colocado en: \n");
                richTextBox1.AppendText(Convert.ToString(dataGridView1.CurrentRow.Index) + "," + Convert.ToString(dataGridView1.CurrentCell.ColumnIndex));

            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (matriz == null)
            {
                MessageBox.Show("Es necesario Definir Primero las Dimenciones de la MATRIZ :)");
            }
            else
            {
                if (textBox3.Text == "" | textBox4.Text == "")
                {
                    MessageBox.Show("Se necesita Definir Cordenadas de Fila y Columna");
                }
                else
                {
                    if (Int32.Parse(textBox3.Text) > matriz.GetLength(0) - 1 | Int32.Parse(textBox4.Text) > matriz.GetLength(1) - 1)
                    {
                        MessageBox.Show("Los valores colocados exexden el tamaño de la dimencion de la matriz, VERIFIQUE");
                    }
                    else
                    {

                        //Validacion para no colocar el obstaculo en el punto de inicio o punto Final
                        if (matriz[Int32.Parse(textBox3.Text), Int32.Parse(textBox4.Text)] == 1 | matriz[Int32.Parse(textBox3.Text), Int32.Parse(textBox4.Text)] == 2)
                        {
                            MessageBox.Show("No se puede Definir Obstaculos en el punto de Inicio o Final");
                        }
                        else
                        {

                            richTextBox1.Clear();
                            //Se define en la matriz el numero 3 como representacion de Obstaculo
                            matriz[Int32.Parse(textBox3.Text), Int32.Parse(textBox4.Text)] = 3;

                            dataGridView1.Rows[Int32.Parse(textBox3.Text)].Cells[Int32.Parse(textBox4.Text)].Style.BackColor = Color.Black;


                            richTextBox1.AppendText("Obstaculo Colocado en: \n");
                            richTextBox1.AppendText(textBox3.Text + "," + textBox4.Text);
                        }
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            rutaX.Clear();
            rutaY.Clear();

            if (matriz == null)
            {
                MessageBox.Show("Definir Dimenciones de Matriz Primero");
            }
            else
            {   //Simple Recorrido  Para verificar la configuracion de la Matriz
                richTextBox1.Clear();
                richTextBox1.AppendText("Matriz Configurada\n\n");
                for (int i = 0; i < matriz.GetLength(0); i++)
                {
                    for (int j = 0; j < matriz.GetLength(1); j++)
                    {
                        richTextBox1.AppendText(Convert.ToString(matriz[i, j]) + " "); ;
                    }
                    richTextBox1.AppendText("\n");
                }
                //Inicia el Proceso de Recorrido 

                // Guardamos las variables que nos van a ser de utilidad para ubicar el Inicio el fin y los obstaculos
                // Variables para la busqueda
                for (int i = 0; i < matriz.GetLength(0); i++)
                {
                    for (int j = 0; j < matriz.GetLength(1); j++)
                    {
                        if (matriz[i, j] == 1)
                        {
                            inicioX = i;
                            inicioY = j;
                        }
                        if (matriz[i, j] == 2)
                        {
                            finX = i;
                            finY = j;
                        }
                        if (matriz[i, j] == 3)
                        {
                            obsX.Add(i); obsY.Add(j);
                        }

                    }
                }
                //Imprime en la Caja de Text las coordenadas de inicio como las finales
                richTextBox1.AppendText("\nPosicion Inicial:  " + inicioX + " , " + inicioY + "\n");
                richTextBox1.AppendText("Posicion Final:  " + finX + " , " + finY + "\n");
                //Imprime en pantalla la posicion de los obstaculos configurados 
                richTextBox1.AppendText("Posicion de Obstaculos:  \n");
                for (int i = 0; i < obsX.Count; i++)
                {
                    richTextBox1.AppendText(Convert.ToString(obsX[i]) + " , " + Convert.ToString(obsY[i]));
                    richTextBox1.AppendText(" \n");
                }
                //Muestra los valores de las cordenadas x e Y de los espacios posibles a Recorrer
                for (int i = 0; i < matriz.GetLength(0); i++)
                {
                    for (int j = 0; j < matriz.GetLength(1); j++)
                    {
                        if ((matriz[i, j] == 0 | matriz[i, j] == 1 & matriz[i, j] != 3))
                        {
                            richTextBox1.AppendText("Camini Encontrado: " + i + " , " + j + "\n");
                            rutaX.Add(i);
                            rutaY.Add(j);
                        }
                    }
                }



            }
        }

        private async void button7_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < rutaX.Count; i++)
            {
                dataGridView1.Rows[rutaX[i]].Cells[rutaY[i]].Style.BackColor = Color.Green;
                await Task.Delay(time1);
                dataGridView1.Rows[rutaX[i]].Cells[rutaY[i]].Style.BackColor = SystemColors.ControlLightLight;
            }

        }

        private async void button8_Click(object sender, EventArgs e)
        {
            richTextBox1.ScrollToCaret();
            int time = 100;
            richTextBox1.Clear();

            //Primer Recorrido
            rX.Clear();
            rY.Clear();
            richTextBox1.AppendText("Recorridos: \n");
            int[] aX = { 3, 2 };
            int[] aY = { 1, 1 };

            for (int i = 0; i < aX.Length; i++)
            {
                rX.Add(aX[i]);
                rY.Add(aY[i]);
            }

            for (int i = 0; i < rX.Count; i++)
            {
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = Color.Green;
                richTextBox1.AppendText("Celda: " + rX[i] + " ," + rY[i] + "\n");
                await Task.Delay(time);
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = SystemColors.ControlLightLight;

            }
            richTextBox1.AppendText("Ruta Con Obstaculo. \n\n");
            cmalos = cmalos + 1;
            rX.Clear();
            rY.Clear();
            //-------------------------------Fin

            //Segundo Recorrido
            rX.Clear();
            rY.Clear();
            richTextBox1.AppendText("Recorridos: \n");
            int[] aX2 = { 3, 3 };
            int[] aY2 = { 1, 2 };

            for (int i = 0; i < aX2.Length; i++)
            {
                rX.Add(aX2[i]);
                rY.Add(aY2[i]);
            }

            for (int i = 0; i < rX.Count; i++)
            {
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = Color.Green;
                richTextBox1.AppendText("Celda: " + rX[i] + " ," + rY[i] + "\n");
                await Task.Delay(time);
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = SystemColors.ControlLightLight;

            }
            richTextBox1.AppendText("Ruta Con Obstaculo. \n\n");
            cmalos = cmalos + 1;
            rX.Clear();
            rY.Clear();
            //-------------------------------Fin

            //Tercer Recorrido
            rX.Clear();
            rY.Clear();
            richTextBox1.AppendText("Recorridos: \n");
            int[] aX3 = { 3, 3, 3, 2, 1, 0, };
            int[] aY3 = { 1, 2, 3, 3, 3, 3 };

            for (int i = 0; i < aX3.Length; i++)
            {
                rX.Add(aX3[i]);
                rY.Add(aY3[i]);
            }

            for (int i = 0; i < rX.Count; i++)
            {
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = Color.Green;
                richTextBox1.AppendText("Celda: " + rX[i] + " ," + rY[i] + "\n");
                await Task.Delay(time);
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = SystemColors.ControlLightLight;

            }
            richTextBox1.AppendText("Ruta Con Tope. \n\n");
            ctope = ctope + 1;
            rX.Clear();
            rY.Clear();
            //-------------------------------Fin

            //Cuarto Recorrido
            rX.Clear();
            rY.Clear();
            richTextBox1.AppendText("Recorridos: \n");
            int[] aX4 = { 3, 3, 3, 3, 2, 1, 0 };
            int[] aY4 = { 1, 2, 3, 4, 4, 4, 4 };

            for (int i = 0; i < aX4.Length; i++)
            {
                rX.Add(aX4[i]);
                rY.Add(aY4[i]);
            }

            for (int i = 0; i < rX.Count; i++)
            {
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = Color.Green;
                richTextBox1.AppendText("Celda: " + rX[i] + " ," + rY[i] + "\n");
                await Task.Delay(time);
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = SystemColors.ControlLightLight;

            }
            richTextBox1.AppendText("Ruta Con Tope. \n\n");
            ctope = ctope + 1;
            rX.Clear();
            rY.Clear();
            //-------------------------------Fin

            //Quinto Recorrido
            rX.Clear();
            rY.Clear();
            richTextBox1.AppendText("Recorridos: \n");
            int[] aX5 = { 3, 3, 3, 3, 3, 2, 1, 0 };
            int[] aY5 = { 1, 2, 3, 4, 5, 5, 5, 5 };

            for (int i = 0; i < aX5.Length; i++)
            {
                rX.Add(aX5[i]);
                rY.Add(aY5[i]);
            }

            for (int i = 0; i < rX.Count; i++)
            {
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = Color.Green;
                richTextBox1.AppendText("Celda: " + rX[i] + " ," + rY[i] + "\n");
                await Task.Delay(time);
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = SystemColors.ControlLightLight;

            }
            richTextBox1.AppendText("Ruta Con Tope. \n\n");
            ctope = ctope + 1;
            rX.Clear();
            rY.Clear();
            //-------------------------------Fin

            //Sexto Recorrido
            rX.Clear();
            rY.Clear();
            richTextBox1.AppendText("Recorridos: \n");
            int[] aX6 = { 3, 3, 3, 3, 3, 3, 2, 1, 0 };
            int[] aY6 = { 1, 2, 3, 4, 5, 6, 6, 6, 6 };

            for (int i = 0; i < aX6.Length; i++)
            {
                rX.Add(aX6[i]);
                rY.Add(aY6[i]);
            }

            for (int i = 0; i < rX.Count; i++)
            {
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = Color.Green;
                richTextBox1.AppendText("Celda: " + rX[i] + " ," + rY[i] + "\n");
                await Task.Delay(time);
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = SystemColors.ControlLightLight;

            }
            richTextBox1.AppendText("Ruta Con Tope. \n\n");
            ctope = ctope + 1;
            rX.Clear();
            rY.Clear();
            //-------------------------------Fin

            //Septimo Recorrido
            rX.Clear();
            rY.Clear();
            richTextBox1.AppendText("Recorridos: \n");
            int[] aX7 = { 3, 3, 3, 3, 3, 3, 3, 2, 1, 0 };
            int[] aY7 = { 1, 2, 3, 4, 5, 6, 7, 7, 7, 7 };

            for (int i = 0; i < aX7.Length; i++)
            {
                rX.Add(aX7[i]);
                rY.Add(aY7[i]);
            }

            for (int i = 0; i < rX.Count; i++)
            {
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = Color.Green;
                richTextBox1.AppendText("Celda: " + rX[i] + " ," + rY[i] + "\n");
                await Task.Delay(time);
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = SystemColors.ControlLightLight;

            }
            richTextBox1.AppendText("Ruta Con Tope. \n\n");
            ctope = ctope + 1;
            rX.Clear();
            rY.Clear();
            //-------------------------------Fin

            //Octavo Recorrido
            rX.Clear();
            rY.Clear();
            richTextBox1.AppendText("Recorridos: \n");
            int[] aX8 = { 3, 3, 3, 3, 3, 3, 3, 3, 2, 1, 0 };
            int[] aY8 = { 1, 2, 3, 4, 5, 6, 7, 8, 8, 8, 8 };

            for (int i = 0; i < aX8.Length; i++)
            {
                rX.Add(aX8[i]);
                rY.Add(aY8[i]);
            }

            for (int i = 0; i < rX.Count; i++)
            {
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = Color.Green;
                richTextBox1.AppendText("Celda: " + rX[i] + " ," + rY[i] + "\n");
                await Task.Delay(time);
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = SystemColors.ControlLightLight;

            }
            richTextBox1.AppendText("Ruta Con Tope. \n\n");
            ctope = ctope + 1;
            rX.Clear();
            rY.Clear();
            //-------------------------------Fin

            //Noveno Recorrido
            rX.Clear();
            rY.Clear();
            richTextBox1.AppendText("Recorridos: \n");
            int[] aX9 = { 3, 3, 3, 3, 3, 3, 3, 3, 3, 2, 1, 0 };
            int[] aY9 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 9, 9, 9 };

            for (int i = 0; i < aX9.Length; i++)
            {
                rX.Add(aX9[i]);
                rY.Add(aY9[i]);
            }

            for (int i = 0; i < rX.Count; i++)
            {
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = Color.Green;
                richTextBox1.AppendText("Celda: " + rX[i] + " ," + rY[i] + "\n");
                await Task.Delay(time);
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = SystemColors.ControlLightLight;

            }
            richTextBox1.AppendText("Ruta Con Tope. \n\n");
            ctope = ctope + 1;
            rX.Clear();
            rY.Clear();
            //-------------------------------Fin

            //Decimo Recorrido
            rX.Clear();
            rY.Clear();
            richTextBox1.AppendText("Recorridos: \n");
            int[] aX10 = { 3, 3, 3, 3, 3, 3, 3, 3, 3 };
            int[] aY10 = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            for (int i = 0; i < aX10.Length; i++)
            {
                rX.Add(aX10[i]);
                rY.Add(aY10[i]);
            }

            for (int i = 0; i < rX.Count; i++)
            {
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = Color.Green;
                richTextBox1.AppendText("Celda: " + rX[i] + " ," + rY[i] + "\n");
                await Task.Delay(time);
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = SystemColors.ControlLightLight;

            }
            richTextBox1.AppendText("Ruta Con Tope. \n\n");
            ctope = ctope + 1;
            rX.Clear();
            rY.Clear();
            //-------------------------------Fin

            //Onceavo Recorrido
            rX.Clear();
            rY.Clear();
            richTextBox1.AppendText("Recorridos: \n");
            int[] aX11 = { 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 5, 6, 7, 8, 9 };
            int[] aY11 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 9, 9, 9, 9, 9, 9 };

            for (int i = 0; i < aX11.Length; i++)
            {
                rX.Add(aX11[i]);
                rY.Add(aY11[i]);
            }

            for (int i = 0; i < rX.Count; i++)
            {
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = Color.Green;
                richTextBox1.AppendText("Celda: " + rX[i] + " ," + rY[i] + "\n");
                await Task.Delay(time);
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = SystemColors.ControlLightLight;

            }
            richTextBox1.AppendText("Ruta Con Tope. \n\n");
            ctope = ctope + 1;
            rX.Clear();
            rY.Clear();
            //-------------------------------Fin

            //Doceavo Recorrido
            rX.Clear();
            rY.Clear();
            richTextBox1.AppendText("Recorridos: \n");
            int[] aX12 = { 3, 3, 3, 3, 3, 3, 3, 3 };
            int[] aY12 = { 1, 2, 3, 4, 5, 6, 7, 8 };

            for (int i = 0; i < aX12.Length; i++)
            {
                rX.Add(aX12[i]);
                rY.Add(aY12[i]);
            }

            for (int i = 0; i < rX.Count; i++)
            {
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = Color.Green;
                richTextBox1.AppendText("Celda: " + rX[i] + " ," + rY[i] + "\n");
                await Task.Delay(time);
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = SystemColors.ControlLightLight;

            }
            richTextBox1.AppendText("Ruta Con Obstaculo. \n\n");
            cmalos = cmalos + 1;
            rX.Clear();
            rY.Clear();
            //-------------------------------Fin

            //Onceavo Recorrido
            rX.Clear();
            rY.Clear();
            richTextBox1.AppendText("Recorridos: \n");
            int[] aX13 = { 3, 3, 3, 3, 3, 3, 3, 4, 5, 6, 7 };
            int[] aY13 = { 1, 2, 3, 4, 5, 6, 7, 7, 7, 7, 7 };

            for (int i = 0; i < aX13.Length; i++)
            {
                rX.Add(aX13[i]);
                rY.Add(aY13[i]);
            }

            for (int i = 0; i < rX.Count; i++)
            {
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = Color.Green;
                richTextBox1.AppendText("Celda: " + rX[i] + " ," + rY[i] + "\n");
                await Task.Delay(time);
                dataGridView1.Rows[rX[i]].Cells[rY[i]].Style.BackColor = SystemColors.ControlLightLight;

            }
            dataGridView1.Rows[7].Cells[7].Style.BackColor = Color.Green;
            await Task.Delay(200);
            dataGridView1.Rows[7].Cells[7].Style.BackColor = SystemColors.ControlLightLight;
            await Task.Delay(200);
            dataGridView1.Rows[7].Cells[7].Style.BackColor = Color.Green;
            await Task.Delay(200);
            dataGridView1.Rows[7].Cells[7].Style.BackColor = SystemColors.ControlLightLight;
            await Task.Delay(200);
            dataGridView1.Rows[7].Cells[7].Style.BackColor = Color.Green;
            await Task.Delay(200);
            dataGridView1.Rows[7].Cells[7].Style.BackColor = SystemColors.ControlLightLight;
            await Task.Delay(200);

            MessageBox.Show("Ruta Encontrada");
            richTextBox1.AppendText("Ruta Exitosa. \n\n");
            cbueno = cbueno + 1;
            rX.Clear();
            rY.Clear();
            //-------------------------------Fin

            //Mostrar el Total de Caminos
            richTextBox1.AppendText("Total Caminos Recorridos: \n\n");
            richTextBox1.AppendText("Caminos Con Tope: " + ctope + " \n");
            richTextBox1.AppendText("Caminos Con Obstaculos: " + cmalos + " \n");
            richTextBox1.AppendText("Caminos Con Exito: " + cbueno + " \n");
        }
    }
}