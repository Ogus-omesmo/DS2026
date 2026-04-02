using System;
using System.Globalization;
using System.Windows.Forms;

namespace Menu_Calculos.Formularios
{
    public partial class frmCalculadoraVisorUnico : Form
    {
        // Estados da calculadora
        private decimal vNumAnt;
        private string vOperacao;
        private bool vLimparVisor;
        private bool vOperacaoRealizada;

        public frmCalculadoraVisorUnico()
        {
            InitializeComponent();
        }

        private void InicializarCalculadora()
        {
            vNumAnt = 0m;
            vOperacao = string.Empty;
            vLimparVisor = false;
            vOperacaoRealizada = false;
            // lblVisor é declarado no Designer; apenas atualizamos o texto
            lblVisor.Text = "0";
        }

        private void f_digitos(object sender, EventArgs e)
        {
            if (!(sender is Button btn))
                return;

            string digito = btn.Text;

            if (lblVisor.Text == "0" && digito == "0")
                return;

            if (vLimparVisor)
            {
                lblVisor.Text = string.Empty;
                vLimparVisor = false;
            }

            if (lblVisor.Text == "0" && digito != "0")
            {
                lblVisor.Text = string.Empty;
            }

            if (lblVisor.Text.Length < 15)
            {
                lblVisor.Text += digito;
            }
        }

        private void f_operacoes(object sender, EventArgs e)
        {
            if (!(sender is Button btn))
                return;

            if (!string.IsNullOrEmpty(vOperacao) && !vOperacaoRealizada)
            {
                // Reutiliza o mesmo fluxo de "=" antes de registrar nova operação
                btnIgual_Click(null, null);
            }

            // Usa TryParse para evitar exceção em caso de formato inesperado
            if (!decimal.TryParse(lblVisor.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out vNumAnt))
            {
                MessageBox.Show("Valor inválido no visor.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                InicializarCalculadora();
                return;
            }

            vOperacao = btn.Text;
            vLimparVisor = true;
            vOperacaoRealizada = false;
        }

        private void btnIgual_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(vOperacao))
                return;

            if (!decimal.TryParse(lblVisor.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal vNumAtual))
            {
                MessageBox.Show("Valor inválido no visor.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                InicializarCalculadora();
                return;
            }

            decimal resultado = 0m;
            bool operacaoValida = true;

            try
            {
                switch (vOperacao)
                {
                    case "+":
                        resultado = vNumAnt + vNumAtual;
                        break;
                    case "-":
                        resultado = vNumAnt - vNumAtual;
                        break;
                    case "×":
                    case "x":
                        resultado = vNumAnt * vNumAtual;
                        break;
                    case "÷":
                    case ":":
                    case "/":
                        if (vNumAtual == 0m)
                        {
                            MessageBox.Show("Divisão por zero não permitida!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            operacaoValida = false;
                        }
                        else
                        {
                            resultado = vNumAnt / vNumAtual;
                        }
                        break;
                    case "%":
                        // caso o botão porcento seja usado como operação binária
                        resultado = vNumAnt % vNumAtual;
                        break;
                    case "^":
                        // potência simples (decimal -> double para Math.Pow)
                        resultado = (decimal)Math.Pow((double)vNumAnt, (double)vNumAtual);
                        break;
                    default:
                        operacaoValida = false;
                        break;
                }

                if (operacaoValida)
                {
                    lblVisor.Text = FormatarResultado(resultado);
                    vOperacao = string.Empty;
                    vLimparVisor = true;
                    vOperacaoRealizada = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro no cálculo: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                InicializarCalculadora();
            }
        }

        private string FormatarResultado(decimal valor)
        {
            // Mantém a cultura corrente para separador decimal
            if (valor == Math.Floor(valor))
                return valor.ToString("F0", CultureInfo.CurrentCulture);

            // mostra até 10 casas decimais, removendo zeros à direita
            return valor.ToString("F10", CultureInfo.CurrentCulture).TrimEnd('0').TrimEnd(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToCharArray());
        }

        private void btnVirgula_Click(object sender, EventArgs e)
        {
            string sep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            if (!lblVisor.Text.Contains(sep))
            {
                if (vLimparVisor)
                {
                    lblVisor.Text = "0";
                    vLimparVisor = false;
                }
                lblVisor.Text += sep;
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            InicializarCalculadora();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (lblVisor.Text.Length > 1)
            {
                lblVisor.Text = lblVisor.Text.Substring(0, lblVisor.Text.Length - 1);
            }
            else
            {
                lblVisor.Text = "0";
            }
        }

        private void btnPorcento_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(lblVisor.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal valor))
            {
                MessageBox.Show("Erro ao calcular porcentagem", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            valor = valor / 100m;
            lblVisor.Text = FormatarResultado(valor);
            vLimparVisor = true;
        }

        private void btnRaizQuadrada_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(lblVisor.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal valor))
            {
                MessageBox.Show("Erro ao calcular raiz quadrada", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (valor < 0m)
            {
                MessageBox.Show("Não é possível calcular raiz de número negativo", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double resultado = Math.Sqrt((double)valor);
            lblVisor.Text = resultado.ToString("F10", CultureInfo.CurrentCulture).TrimEnd('0').TrimEnd(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToCharArray());
            vLimparVisor = true;
        }

        private void btnTrocarSinal_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(lblVisor.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal valor))
            {
                MessageBox.Show("Erro ao trocar sinal", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblVisor.Text = FormatarResultado(valor * -1m);
        }

        private void frmCalculadoraVisorUnico_Load(object sender, EventArgs e)
        {
            // Ajustes visuais podem estar no Designer; apenas inicializamos estado
            InicializarCalculadora();
        }

        // Eventos opcionais (se o Designer ligar a eles)
        private void BotaoBranco_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button btn)
                btn.BackColor = System.Drawing.Color.FromArgb(80, 80, 80);
        }

        private void BotaoBranco_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button btn)
                btn.BackColor = System.Drawing.Color.FromArgb(60, 60, 60);
        }

        private void BotaoOperacao_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button btn)
                btn.BackColor = System.Drawing.Color.FromArgb(120, 120, 120);
        }

        private void BotaoOperacao_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button btn)
                btn.BackColor = System.Drawing.Color.FromArgb(100, 100, 100);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}