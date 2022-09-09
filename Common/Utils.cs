using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common
{
    public class Utils
    {
		private bool ValidaRut(string rut)
		{
			rut = rut.Replace(".", "").ToUpper();
			Regex expresion = new Regex("^([0-9]+-[0-9K])$");
			string dv = rut.Substring(rut.Length - 1, 1);
			if (!expresion.IsMatch(rut))
			{
				return false;
			}
			char[] charCorte = { '-' };
			string[] rutTemp = rut.Split(charCorte);
			if (dv != Digito(int.Parse(rutTemp[0])))
			{
				return false;
			}
			return true;
		}


		/// <summary>
		/// Método que valida el rut con el digito verificador
		/// por separado
		/// </summary>
		/// <param name="rut">integer</param>
		/// <param name="dv">char</param>
		/// <returns>booleano</returns>
		public bool ValidaRut(string rut, string dv)
		{
			return ValidaRut(rut + "-" + dv);
		}

		/// <summary>
		/// método que calcula el digito verificador a partir
		/// de la mantisa del rut
		/// </summary>
		/// <param name="rut"></param>
		/// <returns></returns>
		private string Digito(int rut)
		{
			int suma = 0;
			int multiplicador = 1;
			while (rut != 0)
			{
				multiplicador++;
				if (multiplicador == 8)
					multiplicador = 2;
				suma += (rut % 10) * multiplicador;
				rut = rut / 10;
			}
			suma = 11 - (suma % 11);
			if (suma == 11)
			{
				return "0";
			}
			else if (suma == 10)
			{
				return "K";
			}
			else
			{
				return suma.ToString();
			}
		}

		public string getRut(string rutSinDigito) {
			string digito = Digito(int.Parse(rutSinDigito));
			if (!ValidaRut(rutSinDigito, digito)) { throw new Exception("Rut No valido"); }
			return string.Format("{0}-{1}", rutSinDigito, digito);
		}
	}
}
