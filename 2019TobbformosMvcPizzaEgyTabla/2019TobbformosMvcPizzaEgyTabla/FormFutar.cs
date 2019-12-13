using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TobbbformosPizzaAlkalmazasEgyTabla.Repository;
using TobbbformosPizzaAlkalmazasEgyTabla.Model;
using System.Diagnostics;
using TobbbformosPizzaAlkalmazasEgyTabla.repository;
using TobbbformosPizzaAlkalmazasEgyTabla;

namespace _2019TobbformosMvcPizzaEgyTabla
{
    public partial class FormPizzaFutarKft : Form
    {
        /// <summary>
        /// Futárokat tartalmazó adattábla
        /// </summary>
        private DataTable futarDT = new DataTable();
        /// <summary>
        /// Tárolja a futarokat a listában
        /// </summary>
        private FRepository fr = new FRepository();

        bool ujAdatFel = false;

        private void ujMegsemGombokKezelese()
        {
            if (ujAdatFel == false)
                return;
            if ((textBoxFutarNev.Text != string.Empty) ||
                (textBoxFutarTel.Text != string.Empty))
            {
                buttonFutarUjMentes.Visible = true;
                buttonFutarMegsem.Visible = true;
            }
            else
            {
                buttonFutarUjMentes.Visible = false;
                buttonFutarMegsem.Visible = false;
            }
        }

        private void FutarGombokIndulaskor()
        {
            panelFutar.Visible = false;
            panelModositTorolGombok2.Visible = false;
            if (dataGridViewFutar.SelectedRows.Count != 0)
                buttonUjFutar.Visible = false;
            else
                buttonUjFutar.Visible = true;
        }

        private void beallitGombokatUjMegrendeloMegsemEsMentes()
        {
            if ((dataGridViewFutar.Rows != null) &&
                (dataGridViewFutar.Rows.Count > 0))
                dataGridViewFutar.Rows[0].Selected = true;
            buttonFutarUjMentes.Visible = false;
            buttonFutarMegsem.Visible = false;
            panelModositTorolGombok2.Visible = true;
            ujAdatFel = false;

            textBoxPizzaNev.Text = string.Empty;
            textBoxPizzaAr.Text = string.Empty;
        }

        private void beallitGombokatTextboxokatUjFutarnal()
        {
            panelFutar.Visible = true;
            panelModositTorolGombok2.Visible = false;
            textBoxFutarNev.Text = string.Empty;
            textBoxFutarTel.Text = string.Empty;
        }

        private void KattintaskorGombok()
        {
            ujAdatFel = false;
            buttonFutarUjMentes.Visible = false;
            buttonFutarMegsem.Visible = false;
            panelModositTorolGombok2.Visible = true;
            errorProviderFutarNeve.Clear();
            errorProviderFutarTel.Clear();
        }

        private void updateFutarDGV()
        {
            //Adattáblát feltölti a repoba lévő futár listából
            futarDT = fr.getPizzaDataTableFromList();
            //Pizza DataGridView-nak a forrása a futár adattábla
            dataGridViewFutar.DataSource = null;
            dataGridViewFutar.DataSource = futarDT;
        }

        private void dataGridViewFutar_SelectionChanged(object sender, EventArgs e)
        {
            if (ujAdatFel)
            {
                KattintaskorGombok();
            }
            if (dataGridViewFutar.SelectedRows.Count == 1)
            {
                panelFutar.Visible = true;
                panelModositTorolGombok2.Visible = true;
                buttonUjFutar.Visible = true;
                textBoxFutarAzonosito.Text =
                    dataGridViewFutar.SelectedRows[0].Cells[0].Value.ToString();
                textBoxFutarNev.Text =
                    dataGridViewFutar.SelectedRows[0].Cells[1].Value.ToString();
                textBoxFutarTel.Text =
                    dataGridViewFutar.SelectedRows[0].Cells[2].Value.ToString();
            }
            else
            {
                panelFutar.Visible = false;
                panelModositTorolGombok.Visible = false;
                buttonUjFutar.Visible = false;
            }

        }

        private void setFutarDGV()
        {
            futarDT.Columns[0].ColumnName = "Azonosító";
            futarDT.Columns[0].Caption = "Futár azonosító";
            futarDT.Columns[1].ColumnName = "Név";
            futarDT.Columns[1].Caption = "Futár név";
            futarDT.Columns[2].ColumnName = "Telefonszám";
            futarDT.Columns[2].Caption = "Futár Telefonszám";

            dataGridViewFutar.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dataGridViewFutar.ReadOnly = true;
            dataGridViewFutar.AllowUserToDeleteRows = false;
            dataGridViewFutar.AllowUserToAddRows = false;
            dataGridViewFutar.MultiSelect = false;
        }

        private void buttonFutarokBetoltes_Click(object sender, EventArgs e)
        {
            //Adatbázisban futar tábla kezelése
            RepositoryFutarDatabaseTable rfdt = new RepositoryFutarDatabaseTable();
            //A repo-ba lévő futár listát feltölti az adatbázisból
            fr.setFutarok(rfdt.getFutarFromDatabaseTable());
            updateFutarDGV();
            setFutarDGV();
            FutarGombokIndulaskor();
            dataGridViewFutar.SelectionChanged += dataGridViewFutar_SelectionChanged;
        }

        private void buttonTorolFutar_Click(object sender, EventArgs e)
        {
            torolHibauzenetet();
            if ((dataGridViewFutar.Rows == null) ||
                (dataGridViewFutar.Rows.Count == 0))
                return;
            //A felhasználó által kiválasztott sor a DataGridView-ban            
            int sor = dataGridViewFutar.SelectedRows[0].Index;
            if (MessageBox.Show(
                "Valóban törölni akarja a sort?",
                "Törlés",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                //1. törölni kell a listából
                int id = -1;
                if (!int.TryParse(
                         dataGridViewFutar.SelectedRows[0].Cells[0].Value.ToString(),
                         out id))
                    return;
                try
                {
                    fr.deleteFutarFromListByID(id);
                }
                catch (RepositoryExceptionCantDelete recd)
                {
                    kiirHibauzenetet(recd.Message);
                    Debug.WriteLine("A Futár törlés nem sikerült, nincs a listába!");
                }
                //2. törölni kell az adatbázisból
                RepositoryFutarDatabaseTable rfdt = new RepositoryFutarDatabaseTable();
                try
                {
                    rfdt.deleteFutarFromDatabase(id);
                }
                catch (Exception ex)
                {
                    kiirHibauzenetet(ex.Message);
                }
                //3. frissíteni kell a DataGridView-t  
                updateFutarDGV();
                if (dataGridViewFutar.SelectedRows.Count <= 0)
                {
                    buttonUjFutar.Visible = true;
                }
                setFutarDGV();
            }
        }

        private void buttonModositFutar_Click(object sender, EventArgs e)
        {
            torolHibauzenetet();
            errorProviderFutarNeve.Clear();
            errorProviderFutarTel.Clear();
            try
            {
                Futar modosult = new Futar(
                    Convert.ToInt32(textBoxFutarAzonosito.Text),
                    textBoxFutarNev.Text,
                    textBoxFutarTel.Text
                    );
                int azonosito = Convert.ToInt32(textBoxFutarAzonosito.Text);
                //1. módosítani a listába
                try
                {
                    fr.updateFutarInList(azonosito, modosult);
                }
                catch (Exception ex)
                {
                    kiirHibauzenetet(ex.Message);
                    return;
                }
                //2. módosítani az adatbáziba
                RepositoryFutarDatabaseTable rfdt = new RepositoryFutarDatabaseTable();
                try
                {
                    rfdt.updateFutarInDatabase(azonosito, modosult);
                }
                catch (Exception ex)
                {
                    kiirHibauzenetet(ex.Message);
                }
                //3. módosítani a DataGridView-ban           
                updateFutarDGV();
            }
            catch (ModelFutarNotValidNameExeption fnv)
            {
                errorProviderFutarNeve.SetError(textBoxFutarNev, "Hiba a névben!");
            }
            catch (ModelFutarNotValidTelExeption ftv)
            {
                errorProviderFutarTel.SetError(textBoxFutarTel, "Hiba a Telefonszámban!");
            }
            catch (RepositoryExceptionCantModified recm)
            {
                kiirHibauzenetet(recm.Message);
                Debug.WriteLine("Módosítás nem sikerült, a megrendelő nincs a listába!");
            }
            catch (Exception ex)
            { }
        }

        private void buttonFutarUjMentes_Click(object sender, EventArgs e)
        {
            torolHibauzenetet();
            errorProviderFutarNeve.Clear();
            errorProviderFutarTel.Clear();
            try
            {
                Futar ujF = new Futar(
                    Convert.ToInt32(textBoxFutarAzonosito.Text),
                    textBoxFutarNev.Text,
                    textBoxFutarTel.Text
                    );
                int azonosito = Convert.ToInt32(textBoxFutarAzonosito.Text);
                //1. Hozzáadni a listához
                try
                {

                    fr.addFutarToList(ujF);

                }
                catch (Exception ex)
                {
                    kiirHibauzenetet(ex.Message);
                    return;
                }
                //2. Hozzáadni az adatbázishoz
                RepositoryFutarDatabaseTable rfdt = new RepositoryFutarDatabaseTable();
                try
                {

                    rfdt.insertFutarToDatabase(ujF);

                }
                catch (Exception ex)
                {
                    kiirHibauzenetet(ex.Message);
                }
                //3. Frissíteni a DataGridView-t
                updateFutarDGV();
                beallitGombokatUjMegrendeloMegsemEsMentes();
                if (dataGridViewFutar.SelectedRows.Count == 1)
                {
                    setFutarDGV();
                }

            }
            catch (ModelFutarNotValidNameExeption fnv)
            {
                errorProviderFutarNeve.SetError(textBoxFutarNev, fnv.Message);
            }
            catch (ModelFutarNotValidTelExeption ftv)
            {
                errorProviderFutarTel.SetError(textBoxFutarTel, ftv.Message);
            }
            catch (Exception ex)
            {
            }
        }

        private void buttonUjFutar_Click(object sender, EventArgs e)
        {
            ujAdatFel = true;
            beallitGombokatTextboxokatUjFutarnal();
            int ujFutarAz = fr.getNextFutarId();
            textBoxFutarAzonosito.Text = ujFutarAz.ToString();
        }

        private void buttonFutarMegsem_Click(object sender, EventArgs e)
        {
            beallitGombokatUjMegrendeloMegsemEsMentes();
        }

        private void textBoxFutarNev_TextChanged(object sender, EventArgs e)
        {
            ujMegsemGombokKezelese();
        }

        private void textBoxFutarTel_TextChanged(object sender, EventArgs e)
        {
            ujMegsemGombokKezelese();
        }
    }
}
