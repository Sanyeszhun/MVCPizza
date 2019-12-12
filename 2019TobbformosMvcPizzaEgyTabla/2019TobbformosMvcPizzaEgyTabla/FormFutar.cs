using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TobbformosPizzaAlkalmazasEgyTabla.Repository;
using TobbformosPizzaAlkalmazasEgyTabla.model;

using System.Diagnostics;

namespace _2019TobbformosMvcPizzaEgyTabla
{
    public partial class FormPizzaFutarKft : Form
    {/// <summary>
     /// Megrendelőket tartalmazó adattábla
     /// </summary>
        private DataTable futarDT = new DataTable();
        /// <summary>
        /// Tárolja a megrendelőket listában
        /// </summary>
        private Repository repoo = new Repository();
        bool ujAdat = false;
      

        private void ujMegsemGombokKezelese()
        {
            if (ujAdat == false)
                return;
            if ((textBoxFutarNev.Text != string.Empty) ||
                (textBoxFuttartel.Text != string.Empty))
            {
                buttonUjMentesf.Visible = true;
                buttonMegsemf.Visible = true;
            }
            else
            {
                buttonUjMentesf.Visible = false;
                buttonMegsemf.Visible = false;
            }
        }
        private void FutarGombokIndulaskor()
        {
            panelFutar.Visible = false;
            panelModositTorolGombokfutar.Visible = false;
            if (dataGridViewFutar.SelectedRows.Count != 0)
                buttonUjFutar.Visible = false;
            else
                buttonUjFutar.Visible = true;
            buttonMegsemf.Visible = false;
            buttonUjMentesf.Visible = false;
        }
        private void beallitGombokatUjFutarMegsemEsMentes()
        {
            if ((dataGridViewFutar.Rows != null) &&
                (dataGridViewFutar.Rows.Count > 0))
                dataGridViewFutar.Rows[0].Selected = true;
            buttonUjMentesf.Visible = false;
            buttonMegsemf.Visible = false;
            panelModositTorolGombokfutar.Visible = true;
            ujAdat = false;

            textBoxFuttartel.Text = string.Empty;
            textBoxFutarNev.Text = string.Empty;

        }
        private void beallitGombokatTextboxokatUjFutar()
        {
            panelFutar.Visible = true;
            panelModositTorolGombokfutar.Visible = false;
            textBoxFuttartel.Text = string.Empty;
            textBoxFutarNev.Text = string.Empty;
        }

        private void KattintaskorGombok()
        {
            ujAdat = false;
            buttonUjMentesf.Visible = false;
            buttonMegsemf.Visible = false;
            panelModositTorolGombokfutar.Visible = true;
            errorProviderFutartel.Clear();
            errorProviderFutarnev.Clear();
        }
        private void frissitFutarDGV()
        {
            //Adattáblát feltölti a repoba lévő pizza listából
            futarDT = repoo.getFutarDataTableFromList(); //UnicornsLover
            //Pizza DataGridView-nak a forrása a pizza adattábla
            dataGridViewFutar.DataSource = null;
            dataGridViewFutar.DataSource = futarDT;
        }
        private void dataGridViewFutar_SelectionChanged(object sender, EventArgs e)
        {
            if (ujAdat)
            {
                KattintaskorGombok();
            }
            if (dataGridViewFutar.SelectedRows.Count == 1)
            {
                panelFutar.Visible = true;
                panelModositTorolGombokfutar.Visible = true;
                buttonUjFutar.Visible = true;
                textBoxFutarAzonosito.Text =
                    dataGridViewFutar.SelectedRows[0].Cells[0].Value.ToString();
                textBoxFutarNev.Text =
                    dataGridViewFutar.SelectedRows[0].Cells[1].Value.ToString();
                textBoxFuttartel.Text =
                    dataGridViewFutar.SelectedRows[0].Cells[2].Value.ToString();
            }
            else
            {
                panelFutar.Visible = false;
                panelModositTorolGombokfutar.Visible = false;
                buttonUjFutar.Visible = false;
            }

        }

        private void beallitFutarDGV()
        {
            futarDT.Columns[0].ColumnName = "Azonosító";
            futarDT.Columns[0].Caption = "Megrendelő azonosító";
            futarDT.Columns[1].ColumnName = "Név";
            futarDT.Columns[1].Caption = "Megrendelő név";
            futarDT.Columns[2].ColumnName = "Tel";
            futarDT.Columns[2].Caption = "Telefonszám";

            dataGridViewFutar.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dataGridViewFutar.ReadOnly = true;
            dataGridViewFutar.AllowUserToDeleteRows = false;
            dataGridViewFutar.AllowUserToAddRows = false;
            dataGridViewFutar.MultiSelect = false;
        }
        private void buttonBetoltesFutar_Click(object sender, EventArgs e)
        {
            //Adatbázisban futar tábla kezelése
            RepositoryFutarTableDatabase rfdt = new  RepositoryFutarTableDatabase();
            //A repo-ba lévő futár listát feltölti az adatbázisból
            repoo.setFutar(rfdt.getFutarFromDatabaseTable());
            frissitFutarDGV();
            beallitFutarDGV();
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
                    repoo.deleteFutarFromList(id);
                }
                catch (RepositoryExceptionCantDelete recd)
                {
                    kiirHibauzenetet(recd.Message);
                    Debug.WriteLine("A Megrendelő törlés nem sikerült, nincs a listába!");
                }
                //2. törölni kell az adatbázisból
                RepositoryFutarTableDatabase rdtp = new RepositoryFutarTableDatabase();
                try
                {
                    rdtp.deleteFutarFromDatabase(id);
                }
                catch (Exception ex)
                {
                    kiirHibauzenetet(ex.Message);
                }
                //3. frissíteni kell a DataGridView-t  
                frissitFutarDGV();
                if (dataGridViewFutar.SelectedRows.Count <= 0)
                {
                    buttonUjFutar.Visible = true;
                }
                beallitFutarDGV();
            }
        }

        private void buttonModositFutar_Click(object sender, EventArgs e)
        {
            torolHibauzenetet();
            errorProviderFutarnev.Clear();
            errorProviderFutartel.Clear();
            try
            {
                Futar modosult = new Futar(
                    Convert.ToInt32(textBoxFutarAzonosito.Text),
                    textBoxFutarNev.Text,
                    textBoxFuttartel.Text
                    );
                int azonosito = Convert.ToInt32(textBoxFutarAzonosito.Text);
                //1. módosítani a listába
                try
                {
                    repoo.updateFutarInList(azonosito, modosult);
                }
                catch (Exception ex)
                {
                    kiirHibauzenetet(ex.Message);
                    return;
                }
                //2. módosítani az adatbáziba
                RepositoryFutarTableDatabase rdtp = new RepositoryFutarTableDatabase();
                try
                {
                    rdtp.updateFutarInDatabase(azonosito, modosult);
                }
                catch (Exception ex)
                {
                    kiirHibauzenetet(ex.Message);
                }
                //3. módosítani a DataGridView-ban           
                frissitFutarDGV();
            }
            catch (ModelFutarNotValidNameExeption mnv)
            {
                errorProviderFutarnev.SetError(textBoxFutarNev, "Hiba a névben!");
            }
            catch (ModelFutarNotValidTelExeption mcv)
            {
                errorProviderFutartel.SetError(textBoxFuttartel, "Hiba a címben!");
            }
            catch (RepositoryExceptionCantModified recm)
            {
                kiirHibauzenetet(recm.Message);
                Debug.WriteLine("Módosítás nem sikerült, a megrendelő nincs a listába!");
            }
            catch (Exception ex)
            { }
        }
        private void buttonUjMentesf_Click(object sender, EventArgs e)
        {
            torolHibauzenetet();
            errorProviderFutartel.Clear();
            errorProviderFutarnev.Clear();
            try
            {
                Futar ujM = new Futar(
                    Convert.ToInt32(textBoxFutarAzonosito.Text),
                    textBoxFutarNev.Text,
                    textBoxFuttartel.Text
                    );
                int azonosito = Convert.ToInt32(textBoxFutarAzonosito.Text);
                //1. Hozzáadni a listához
                try
                {

                    repoo.addFutarToList(ujM);

                }
                catch (Exception ex)
                {
                    kiirHibauzenetet(ex.Message);
                    return;
                }
                //2. Hozzáadni az adatbázishoz
                RepositoryFutarTableDatabase rdtp = new RepositoryFutarTableDatabase();
                try
                {

                    rdtp.insertFutarToDatabase(ujM);

                }
                catch (Exception ex)
                {
                    kiirHibauzenetet(ex.Message);
                }
                //3. Frissíteni a DataGridView-t
                frissitFutarDGV();
                beallitGombokatUjFutarMegsemEsMentes();
                if (dataGridViewFutar.SelectedRows.Count == 1)
                {
                    beallitFutarDGV();
                }

            }
            catch (ModelFutarNotValidNameExeption mnv)
            {
                errorProviderFutarnev.SetError(textBoxFutarNev, "Hiba a névben!");
            }
            catch (ModelFutarNotValidTelExeption mcv)
            {
                errorProviderFutartel.SetError(textBoxFuttartel, "Hiba a címben!");
            }
            catch (Exception ex)
            {
            }
        }
        private void buttonUjFutar_Click(object sender, EventArgs e)
        {
            ujAdat = true;
            beallitGombokatTextboxokatUjFutar();
            int ujFutarAz = repoo.getNextPFutarId();
            textBoxFutarAzonosito.Text = ujFutarAz.ToString();
        }

        private void buttonMegsemf_Click(object sender, EventArgs e)
        {
            beallitGombokatUjFutarMegsemEsMentes();
        }
        private void textBoxFutarNev_TextChanged(object sender, EventArgs e)
        {
            ujMegsemGombokKezelese();
        }

        private void textBoxFuttartel_TextChanged(object sender, EventArgs e)
        {
            ujMegsemGombokKezelese();
        }
    }
}
