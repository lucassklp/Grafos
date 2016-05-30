using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;
using SdlDotNet.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EdmondsKarp
{
    public partial class Form1 : Form
    {
        //A surface é a representação da tela
        //A tela principal é a Screen, e agrega todas as outras
        private Surface Screen;
        private Surface SurfaceTextCoordinate;
        private Surface SurfaceTextStatus;
        private Surface SurfaceTextMenu;

        //Esquema de cores
        Color backgroundColor = Color.White;
        Color foregroundColor = Color.Black;

        //Exemplo de uma instância de cor RGBA
        Color customColor = Color.FromArgb(8, 255, 0, 0);

        //Variáveis de estado do programa
        private static int x = 0; //Coordenada X
        private static int y = 0; //Coordenada Y
        private char PreviousCommand = ' '; //Comando Anterior
        private static char Command = ' '; //Proximo Comando

        //Legendas
        private string Status = "Ready"; //Texto do status
        private string Menu = "M: Mover | E: Inserir novo nó | Delete: Deletar Elementos | T: Adicionar Vértice | O: Definir Origem | D: Definir destino"; //Texto do Menu


        //Representação global dos automatos
        private List<char> Alphabet; //Alfabeto
        Triangle sourceNode = new Triangle(new Point(0, 0), new Point(10, 10), new Point(0, 20)); //Seta do Estado Inicial
        Triangle destinationNode= new Triangle(new Point(0, 0), new Point(10, 10), new Point(0, 20)); //Seta do Estado Inicial



        List<Vertex> listVertex = new List<Vertex>(); //Lista com os Nós (Ou Estados)
        List<Edge> listEdge = new List<Edge>(); //Lista com as transições

        //Nodes temporários (usados no comando de transição)
        Vertex Source = null, Destination = null, Selected = null, From = null, To = null;

        private Line linhaTransicao;
        private Triangle setaTransicao;

        public Form1()
        {
            InitializeComponent();
            this.Hide();
            IniciarSdl();

        }

        public void IniciarSdl()
        {
            Screen = Video.SetVideoMode(1024, 500, false, false, false);
            Video.WindowCaption = "Simulador de Grafos - Fluxo Máximo";
            SdlDotNet.Graphics.Font font = new SdlDotNet.Graphics.Font(@"C:\Windows\Fonts\Arial.ttf", 12);

            SdlDotNet.Core.Events.Fps = 60;

            SdlDotNet.Core.Events.Tick += new EventHandler<TickEventArgs>(delegate(object sender, TickEventArgs args)
            {
                SdlDotNet.Core.Events.MouseButtonDown += new EventHandler<MouseButtonEventArgs>(MouseButtonDown);
                SdlDotNet.Core.Events.MouseMotion += new EventHandler<MouseMotionEventArgs>(MouseMotionEvent);
                SdlDotNet.Core.Events.KeyboardUp += new EventHandler<KeyboardEventArgs>(KeyboardPress);
                SdlDotNet.Core.Events.MouseButtonUp += new EventHandler<MouseButtonEventArgs>(MouseButtonUp);

                string Position = string.Format("Posição X: {0} Y: {1}", x, y);
                Status = GetStatus(Command);

                Screen.Fill(backgroundColor);























                //Exibir as linhas de transição
                List<Edge> transicoesFeitas = new List<Edge>();
                foreach (var item in this.listEdge)
                {


                    Line p = new Line(item.From.Coordenada, item.To.Coordenada);
                    Point pontoMedio = Calculos.GetPontoMedio(p.Point1, p.Point2);
                    Point centroCircunferencia = item.To.Coordenada;



                    //Imprime a linha
                    p.Draw(Screen, foregroundColor, true);

                    //Imprime o elemento de transição
                    string element = string.Format("{0} / {1}", item.Load, item.Capacity);
                    Surface transitionElementText = font.Render(element, foregroundColor);
                    Screen.Blit(transitionElementText, pontoMedio);

                    //Pegamos a posição da seta
                    Point setaPosition = Calculos.GetSetaPosition(p, centroCircunferencia);
                    int angulo = Calculos.GetAnguloReta(p.Point1, p.Point2);

                    Triangle seta = Calculos.GetArrow(setaPosition, angulo);
                    seta.Draw(Screen, foregroundColor, true, true);


                    //







                    transicoesFeitas.Add(item);
                }

                


                if (this.linhaTransicao != null)
                {
                    this.linhaTransicao.Draw(Screen, Color.Red);
                    this.setaTransicao.Draw(Screen, Color.Red, true, true);
                }

                //Exibir automatos
                foreach (var item in this.listVertex)
                {
                    Circle node = new Circle(item.Coordenada, 20); //Centro do circulo é a coordenada dele, e o raio é 20

                    node.Draw(Screen, Color.Green, true, true);
                    Surface nodeName = font.Render(item.Nome, Color.White);
                    Screen.Blit(nodeName, new Point(item.Coordenada.X - 7, item.Coordenada.Y - 7));



                    if (this.Source != null) 
                    { 
                        if (item.Nome == this.Source.Nome)
                        {
                            sourceNode.Center = new Point(item.Coordenada.X - 20, item.Coordenada.Y);
                            sourceNode.Draw(Screen, Color.Red, true, true);
                        }
                    }
                    if (this.Destination != null) 
                    {
                        if (item.Nome == this.Destination.Nome)
                        {
                            sourceNode.Center = new Point(item.Coordenada.X - 20, item.Coordenada.Y);
                            sourceNode.Draw(Screen, Color.Violet, true, true);
                        }
                    }



                }




                if (Command == 'Q')
                {
                    Graph p = new Graph(this.listVertex, this.listEdge);
                    EdmondsKarp alg = new EdmondsKarp(p);
                    alg.FindMaxFlow(this.Source, this.Destination);
                    int maxFlow = alg.GetMaxFlow(this.Destination);

                    MessageBox.Show("Fluxo máximo: " + maxFlow, "Sucesso");


                    Command = ' ';
                    
                }




                if (Command == 'S')
                {
                    Command = ' ';

                    Stream myStream;
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.AddExtension = true;
                    sfd.DefaultExt = "eka";
                    sfd.Filter = "Edmonds Karp Algorithm (*.eka) | *.eka";
                    sfd.FileName = "Untitled";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            if ((myStream = sfd.OpenFile()) != null)
                            {
                                Graph p = new Graph(this.listVertex, this.listEdge);
                                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                                binaryFormatter.Serialize(myStream, p);
                                MessageBox.Show("Salvo com sucesso!");

                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erro: " + ex.Message);
                        }
                    }


                }
                else if (Command == 'A')
                {
                    Command = ' ';

                    Stream myStream;

                    OpenFileDialog sfd = new OpenFileDialog();
                    sfd.AddExtension = true;
                    sfd.DefaultExt = "eka";
                    sfd.Filter = "Edmonds Karp Algorithm (*.eka) | *.eka";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            if ((myStream = sfd.OpenFile()) != null)
                            {
                                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                                Graph p = (Graph)binaryFormatter.Deserialize(myStream);
                                this.listVertex.Clear();
                                this.listEdge.Clear();


                                this.listEdge = p.ListEdges;
                                this.listVertex = p.ListVertex;

                                MessageBox.Show("Carregado com sucesso!");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erro: " + ex.Message);
                        }
                    }
                }


                SurfaceTextCoordinate = font.Render(Position, foregroundColor);
                SurfaceTextStatus = font.Render(Status, foregroundColor);
                SurfaceTextMenu = font.Render(this.Menu, foregroundColor);
                
                Screen.Blit(SurfaceTextCoordinate, new Point(890, 0));
                Screen.Blit(SurfaceTextMenu, new Point(0, 0));
                Screen.Blit(SurfaceTextStatus, new Point(0, 450));
                

                Screen.Update();
            });

            SdlDotNet.Core.Events.Run();
        }

        private void MouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Command == 'M')
                this.Selected = null;

            else if (Command == 'T')
            {
                Vertex selectedNode = GetClickedNode();
                if (selectedNode != null)
                {
                    int value;

                    GetElementTransition getElementTransition = new GetElementTransition();
                    getElementTransition.ShowDialog();
                    if (!getElementTransition.isCanceled)
                    {
                        value = getElementTransition.Element;
                        this.listEdge.Add(new Edge(From, selectedNode, value));
                    }
                    getElementTransition.Close();
                }

                this.linhaTransicao = new Line();
                this.setaTransicao = new Triangle();
                this.From = null;
                this.To = null;
                Command = ' ';
            }

        }

        private void KeyboardPress(object sender, KeyboardEventArgs e)
        {
            if (e.Key == (Key.Delete))
                Command = 'D';
            else if (e.Key == (Key.E))
                Command = 'E';
            else if (e.Key == (Key.T))
                Command = 'T';
            else if (e.Key == (Key.S))
                Command = 'S';
            else if (e.Key == (Key.M))
                Command = 'M';
            else if (e.Key == (Key.K))
                Command = 'K';
            else if (e.Key == (Key.O))
                Command = 'O';
            else if (e.Key == (Key.D))
                Command = 'J';
            else if (e.Key == (Key.Q))
                Command = 'Q';
            else if (e.Key == (Key.A))
                Command = 'A';
        }

        private void MouseButtonDown(object sender, MouseButtonEventArgs args)
        {
            if (args.Button == MouseButton.PrimaryButton)
            {
                if (Command == 'E')
                {
                    string Nome = GenerateNodeName();
                    this.listVertex.Add(new Vertex(Nome, new Point(x, y)));
                    this.PreviousCommand = 'E';
                    Command = ' ';
                }
                else if (Command == 'D')
                {
                    Vertex toDelete = GetClickedNode();
                    Edge TransicionToDelete = GetClickedTransition();


                    if (toDelete != null)
                    {
                        this.listEdge.RemoveAll(p => p.From.Nome == toDelete.Nome || p.To.Nome == toDelete.Nome);
                        listVertex.Remove(toDelete);
                    }
                    if (TransicionToDelete != null)
                        this.listEdge.Remove(TransicionToDelete);


                    this.PreviousCommand = 'D';
                    Command = ' ';
                }
                else if (Command == 'T' && this.From == null)
                {
                    this.From = GetClickedNode();
                }
                else if (Command == 'S' && this.PreviousCommand != 'T')
                {
                    this.PreviousCommand = 'S';
                    Command = ' ';
                    return;
                }
                else if (Command == 'M')
                {
                    this.Selected = GetClickedNode();
                    if (this.Selected != null)
                        SdlDotNet.Core.Events.MouseMotion += new EventHandler<MouseMotionEventArgs>(ArrastarNode);
                    else
                        return;

                }
                else if (Command == 'O')
                {
                    Vertex setSource = GetClickedNode();
                    this.Source = setSource;

                    Command = ' ';
                }
                else if (Command == 'J')
                {
                    Vertex setDestination = GetClickedNode();
                    this.Destination = setDestination;
                    Command = ' ';
                }










            }
        }



        private void ArrastarNode(object sender, MouseMotionEventArgs e)
        {
            if (this.Selected != null)
            {
                this.Selected.Coordenada.X = x;
                this.Selected.Coordenada.Y = y;
            }
        }

        private Vertex GetClickedNode()
        {

            foreach (var item in this.listVertex)
            {
                //Tomemos o centro da circunferência:
                Point centroCircunferência = item.Coordenada;
                Point pontoAtual = new Point(x, y);

                if (Calculos.PontoPertenceACircunferencia(pontoAtual, centroCircunferência))
                    return item;
            }

            return null;
        }

        private Edge GetClickedTransition()
        {
            foreach (var item in this.listEdge)
            {
                Point pontoAtual = new Point(x, y);
                Line linha = new Line(item.From.Coordenada, item.To.Coordenada);

                if (Calculos.VerificarSePontoPertenceAReta(linha, pontoAtual))
                {
                    if (linha.Point2.X < linha.Point1.X && linha.Point2.Y < linha.Point1.Y)
                    {
                        if ((pontoAtual.X > linha.Point2.X && pontoAtual.Y > linha.Point2.Y) && (pontoAtual.X < linha.Point1.X && pontoAtual.Y < linha.Point1.Y))
                            return item;
                    }
                    else
                        if ((pontoAtual.X < linha.Point2.X && pontoAtual.Y < linha.Point2.Y) && (pontoAtual.X > linha.Point1.X && pontoAtual.Y > linha.Point1.Y))
                            return item;

                }
            }

            return null;
        }

        private string GenerateNodeName()
        {
            int count = 0;
            for (count = 0; count < listVertex.Count; count++)
            {
                if (this.listVertex.Find(p => p.Nome == string.Format("{0}", count)) == null)
                    return string.Format("{0}", count);
            }

            return string.Format("{0}", count);
        }

        private void MouseMotionEvent(object sender, MouseMotionEventArgs args)
        {
            x = args.Position.X;
            y = args.Position.Y;

            if (From != null && Command == 'T')
            {
                this.linhaTransicao = new Line(this.From.Coordenada, new Point(x, y));
                int angulo = Calculos.GetAnguloReta(this.linhaTransicao.Point1, this.linhaTransicao.Point2);
                this.setaTransicao = Calculos.GetArrow(new Point(x, y), angulo);

                
            }

        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            SdlDotNet.Core.Events.QuitApplication();
        }

        public string GetStatus(char op)
        {
            if (op == 'D')
                return "Clique no elemento para deletar!";
            else if (op == 'E')
                return "Clique para adicionar um novo estado!";
            else if (op == 'M')
                return "Clique no Item, e mova para a nova posição";
            else if (op == 'T')
                return "Digite a letra do alfabeto, clique no estado de origem, e então pressione S";
            else if (op == 'S' && this.PreviousCommand == 'T')
                return "Clique no estado de destino";
            else if (op == 'S' && this.PreviousCommand != 'T')
                return "Para adicionar uma transição, pressione T primeiramente";
            else if (op == 'I')
                return "Clique no estado que deseja tornar inicial";
            else return "Ready";
        }
    }
}
