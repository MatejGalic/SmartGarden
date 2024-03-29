﻿using SmartGarden.Models;

namespace SmartGarden.Services.Calculator
{
    public class CalculatorService : ICalculatorService
    {
        public SolarPanelResults CalculateEnergyProduction(SolarPanelParameters parameters)
        {
            double fi_rad = ToRadians(parameters.Latitude);
            double beta_rad = ToRadians(parameters.PanelTilt);
            double omega_c = 0;

            int n = new DateTime(parameters.Year, parameters.Month, parameters.Day).DayOfYear;

            double dekl = 23.45 * Math.Sin(2 * Math.PI * (284 + n) / 365);
            double dekl_rad = ToRadians(dekl);

            double A = 1160 + 75 * Math.Sin(2 * Math.PI / 365 * (n - 275));
            double k = 0.174 + 0.035 * Math.Sin(2 * Math.PI / 365 * (n - 100));
            double C = 0.095 + 0.04 * Math.Sin(2 * Math.PI / 365 * (n - 100));

            double ws_rad = Math.Acos(-Math.Tan(fi_rad) * Math.Tan(dekl_rad));
            double ws_deg = ToDegrees(ws_rad);

            double t_iz = 12 - ws_deg / 15;
            double t_zal = 12 + ws_deg / 15;
            double t_sun = t_zal - t_iz;

            int num_intervals = 10;

            double cumulative_energy = 0;
            double cumulative_energy_efficiency = 0;

            for (int i = 1; i <= num_intervals; i++)
            {
                double t_start_i = t_iz + (i - 1) * (t_sun / num_intervals);
                double t_end_i = t_iz + i * (t_sun / num_intervals);
                double t_med_i = t_start_i + (t_end_i - t_start_i) / 2;

                double w = (12 - t_med_i) * 15;
                double w_rad = ToRadians(w);

                double sin_alpha = Math.Sin(fi_rad) * Math.Sin(dekl_rad) + Math.Cos(fi_rad) * Math.Cos(dekl_rad) * Math.Cos(w_rad);
                double alpha = Math.Asin(sin_alpha);

                double m = 1 / sin_alpha;
                double Gb = A * Math.Exp(-k * m);
                double Gd = C * Gb;

                double omega_s_deg;
                if (Math.Cos(w_rad) >= (Math.Tan(dekl_rad) / Math.Tan(fi_rad)))
                {
                    omega_s_deg = ToDegrees(Math.Asin(Math.Cos(dekl_rad) * Math.Sin(w_rad) / Math.Cos(alpha)));
                }
                else
                {
                    omega_s_deg = 180 - ToDegrees((Math.Asin(Math.Cos(dekl_rad) * Math.Sin(w_rad) / Math.Cos(alpha))));
                }

                double cos_fi_beta = Math.Cos(alpha) * Math.Cos(ToRadians(omega_s_deg) - omega_c) * Math.Sin(beta_rad) + Math.Sin(alpha) * Math.Cos(beta_rad);
                double Gb_beta = Gb * cos_fi_beta;
                double Gd_beta = Gd * ((1 + Math.Cos(beta_rad)) / 2);
                double Gr_beta = 0.2 * (Gd + Gb) * ((1 - Math.Cos(beta_rad)) / 2);
                double G_beta = Gb_beta + Gd_beta + Gr_beta;

                double T_cell = parameters.AmbientTemperature + ((parameters.NOCT - 20) / 800) * G_beta;
                double P = parameters.PanelPower * (G_beta / 1000) * (1 - (Math.Abs(parameters.PowerTemperatureCoefficient)) / 100 * (T_cell - 25));

                double W = P * (t_end_i - t_start_i);
                cumulative_energy += W;

                double H = G_beta * (t_end_i - t_start_i) * parameters.PanelArea;
                double E = H * (parameters.PanelEfficiency / 100);
                cumulative_energy_efficiency += E;
            }

            return new SolarPanelResults
            {
                CumulativeEnergy = cumulative_energy,
                CumulativeEnergyEfficiency = cumulative_energy_efficiency
            };
        }

        private double ToRadians(double degrees)
        {
            return Math.PI / 180 * degrees;
        }

        private double ToDegrees(double radians)
        {
            return 180 / Math.PI * radians;
        }
    }
}
