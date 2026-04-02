using System;

namespace Calculadora{
    public class frmCalculadoraVisorUnico
    {
        // Variable to store the result.
        private double resultado;

        // Method to perform addition.
        public double Sumar(double a, double b)
        {
            resultado = a + b;
            return resultado;
        }

        // Method to perform subtraction.
        public double Restar(double a, double b)
        {
            resultado = a - b;
            return resultado;
        }

        // Method to perform multiplication.
        public double Multiplicar(double a, double b)
        {
            resultado = a * b;
            return resultado;
        }

        // Method to perform division.
        public double Dividir(double a, double b)
        {
            if (b == 0)
            {
                throw new DivideByZeroException();
            }
            resultado = a / b;
            return resultado;
        }
    }
}