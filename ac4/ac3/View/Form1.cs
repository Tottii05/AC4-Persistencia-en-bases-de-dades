using ac3.Business.DTOs;
using ac3.Business.Entities;
using ac3.Business.Utils;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace ac3
{
    public partial class Form1 : Form
    {
        IContactDAO contactDAO = new ContactDAO(NpgsqlUtils.OpenConnection());
        public Form1()
        {
            InitializeComponent();
            FillYearComboBox();
            FillComarcaComboBox();
        }
        private static List<Consum> GetInfoCsv(string path, DataGridView dataGridView)
        {
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                var records = csv.GetRecords<Consum>().ToList();
                return records;
            }
        }
        private void ReloadDataGrid()
        {
            comarquesDataGrid.Rows.Clear();
            comarquesDataGrid.Columns.Clear();

            comarquesDataGrid.Columns.Add("Any", "Any");
            comarquesDataGrid.Columns.Add("Comarca", "Comarca");
            comarquesDataGrid.Columns.Add("Poblaci�", "Poblaci�");
            comarquesDataGrid.Columns.Add("Dom�stic xarxa", "Dom�stic xarxa");
            comarquesDataGrid.Columns.Add("A.E. i fonts pr�pies", "A.E. i fonts pr�pies");
            comarquesDataGrid.Columns.Add("Total", "Total");
            comarquesDataGrid.Columns.Add("Consum dom�stic/c�pita", "Consum dom�stic/c�pita");

            List<ConsumDTO> consums = contactDAO.GetAllConsum();

            foreach (var consumo in consums)
            {
                int rowIndex = comarquesDataGrid.Rows.Add();
                DataGridViewRow row = comarquesDataGrid.Rows[rowIndex];

                row.Cells["Any"].Value = consumo.Any;
                row.Cells["Comarca"].Value = consumo.Comarca;
                row.Cells["Poblaci�"].Value = consumo.Poblacio;
                row.Cells["Dom�stic xarxa"].Value = consumo.Domestic_xarxa;
                row.Cells["A.E. i fonts pr�pies"].Value = consumo.Activitats_economiques_i_fonts_propies;
                row.Cells["Total"].Value = consumo.Total;
                row.Cells["Consum dom�stic/c�pita"].Value = consumo.Consum_domestic_per_capita;
            }
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            yearComboBox.Text = string.Empty;
            comarcaComboBox.Text = string.Empty;
            poblationTextBox.Text = string.Empty;
            domesticXarxaTextBox.Text = string.Empty;
            activitiesTextBox.Text = string.Empty;
            totalTextBox.Text = string.Empty;
            domesticCapitaTextBox.Text = string.Empty;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<ConsumDTO> consums = contactDAO.GetAllConsum();

            comarquesDataGrid.Rows.Clear();
            comarquesDataGrid.Columns.Clear();

            comarquesDataGrid.Columns.Add("Any", "Any");
            comarquesDataGrid.Columns.Add("Comarca", "Comarca");
            comarquesDataGrid.Columns.Add("Poblaci�", "Poblaci�");
            comarquesDataGrid.Columns.Add("Dom�stic xarxa", "Dom�stic xarxa");
            comarquesDataGrid.Columns.Add("A.E. i fonts pr�pies", "A.E. i fonts pr�pies");
            comarquesDataGrid.Columns.Add("Total", "Total");
            comarquesDataGrid.Columns.Add("Consum dom�stic/c�pita", "Consum dom�stic/c�pita");

            foreach (var consumo in consums)
            {
                int rowIndex = comarquesDataGrid.Rows.Add();
                DataGridViewRow row = comarquesDataGrid.Rows[rowIndex];

                row.Cells["Any"].Value = consumo.Any;
                row.Cells["Comarca"].Value = consumo.Comarca;
                row.Cells["Poblaci�"].Value = consumo.Poblacio;
                row.Cells["Dom�stic xarxa"].Value = consumo.Domestic_xarxa;
                row.Cells["A.E. i fonts pr�pies"].Value = consumo.Activitats_economiques_i_fonts_propies;
                row.Cells["Total"].Value = consumo.Total;
                row.Cells["Consum dom�stic/c�pita"].Value = consumo.Consum_domestic_per_capita;
            }
        }

        private void FillYearComboBox()
        {
            const int MaxYear = 2050;
            List<Consum> consums = GetInfoCsv("../../../files/Consum_d_aigua_a_Catalunya_per_comarques_20240402.csv", comarquesDataGrid);
            int oldestYear = consums.Min(x => x.Any);
            for (int i = oldestYear; i <= MaxYear; i++)
            {
                yearComboBox.Items.Add(i);
            }
        }

        private void FillComarcaComboBox()
        {
            List<string> comarquesNames = Helper.GetComarquesXml("../../../files/Consum_d_aigua_a_Catalunya_per_comarques_20240402.xml");
            foreach (var comarca in comarquesNames)
            {
                comarcaComboBox.Items.Add(comarca);
            }
        }

        private void comarquesDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            const int PopulationLimiter = 20000;

            List<Consum> listConsum = GetInfoCsv("../../../files/Consum_d_aigua_a_Catalunya_per_comarques_20240402.csv", comarquesDataGrid);

            biggerPoblationShowValue.Visible = true;
            domesticAverageShowValue.Visible = true;
            biggestConsumShowValue.Visible = true;
            lowestConsumShowValue.Visible = true;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = comarquesDataGrid.Rows[e.RowIndex];
                int poblacion;
                biggerPoblationShowValue.Text = int.TryParse(row.Cells["Poblaci�"].Value.ToString(), out poblacion) && poblacion > PopulationLimiter ? "S�" : "No";
                double.TryParse(row.Cells["Dom�stic xarxa"].Value.ToString(), out double domesticXarxa);
                double.TryParse(row.Cells["Poblaci�"].Value.ToString(), out double poblacionDouble);
                double average = poblacionDouble != 0 ? domesticXarxa / poblacionDouble : 0;
                domesticAverageShowValue.Text = average.ToString();
                int.TryParse(row.Cells["Consum dom�stic/c�pita"].Value.ToString(), out int consumPerCapita);
                bool isBiggestConsumPerCapita = consumPerCapita == listConsum.Max(x => x.Consum_domestic_per_capita);
                biggestConsumShowValue.Text = isBiggestConsumPerCapita ? "S�" : "No";
                bool isLowestConsumPerCapita = consumPerCapita == listConsum.Min(x => x.Consum_domestic_per_capita);
                lowestConsumShowValue.Text = isLowestConsumPerCapita ? "S�" : "No";
            }
        }
        private void PersistenceBtn_Click(object sender, EventArgs e)
        {
            int year, poblacion, domesticXarxa, activities, total;
            double consumPerCapita;
            string comarca;

            yearErrorProvider.Clear();
            comarcaErrorProvider.Clear();
            poblationErrorProvider.Clear();
            domesticXarxaErrorProvider.Clear();
            AEErrorProvider.Clear();
            PerCapitaErrorProvider.Clear();
            TotalErrorProvider.Clear();

            try
            {
                year = int.Parse(yearComboBox.Text);
                if (year < 0)
                    throw new Exception();
            }
            catch (Exception)
            {
                yearErrorProvider.SetError(yearComboBox, "El a�o debe ser un n�mero positivo y no estar vac�o.");
                return;
            }

            try
            {
                comarca = comarcaComboBox.Text;
                if (comarca == string.Empty)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                comarcaErrorProvider.SetError(comarcaComboBox, "La comarca no puede estar vac�a.");
                return;
            }

            try
            {
                poblacion = int.Parse(poblationTextBox.Text);
                if (poblacion < 0)
                    throw new Exception();
                poblationErrorProvider.Clear();
            }
            catch (Exception)
            {
                poblationErrorProvider.SetError(poblationTextBox, "La poblaci�n debe ser un n�mero positivo y no estar vac�a.");
                return;
            }

            try
            {
                domesticXarxa = int.Parse(domesticXarxaTextBox.Text);
                if (domesticXarxa < 0)
                    throw new Exception();
                domesticXarxaErrorProvider.Clear();
            }
            catch (Exception)
            {
                domesticXarxaErrorProvider.SetError(domesticXarxaTextBox, "El consumo dom�stico de la red debe ser un n�mero positivo y no estar vac�o.");
                return;
            }

            try
            {
                activities = int.Parse(activitiesTextBox.Text);
                if (activities < 0)
                    throw new Exception();
                AEErrorProvider.Clear();
            }
            catch (Exception)
            {
                AEErrorProvider.SetError(activitiesTextBox, "El consumo de actividades econ�micas y fuentes propias debe ser un n�mero positivo y no estar vac�o.");
                return;
            }

            try
            {
                consumPerCapita = double.Parse(domesticCapitaTextBox.Text);
                if (consumPerCapita < 0)
                    throw new Exception();
                PerCapitaErrorProvider.Clear();
            }
            catch (Exception)
            {
                PerCapitaErrorProvider.SetError(domesticCapitaTextBox, "El consumo dom�stico per c�pita debe ser un n�mero positivo y no estar vac�o.");
                return;
            }

            try
            {
                total = int.Parse(textBox3.Text);
                if (total < 0)
                    throw new Exception();
                TotalErrorProvider.Clear();
            }
            catch (Exception)
            {
                TotalErrorProvider.SetError(totalTextBox, "El consumo total debe ser un n�mero positivo y no estar vac�o.");
                return;
            }

            ConsumDTO consume = new ConsumDTO
            {
                Any = year,
                Comarca = comarcaComboBox.Text,
                Codi_Comarca = Helper.GetCodiComarca(comarca),
                Poblacio = poblacion,
                Domestic_xarxa = domesticXarxa,
                Activitats_economiques_i_fonts_propies = activities,
                Total = total,
                Consum_domestic_per_capita = consumPerCapita
            };
            contactDAO.AddConsum(consume);
            ReloadDataGrid();
        }
    }
}
