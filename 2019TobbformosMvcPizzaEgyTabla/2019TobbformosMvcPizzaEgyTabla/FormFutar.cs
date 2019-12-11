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
        private DataTable megrendeloDT = new DataTable();
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
        private void MegrendeloGombokIndulaskor()
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
        private void beallitGombokatUjMegrendeloMegsemEsMentes()
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
        private void beallitGombokatTextboxokatUjMegrendelonel()
        {
            panelFutar.Visible = true;
            panelModositTorolGombokfutar.Visible = false;
            textBoxMegrendeloCim.Text = string.Empty;
            textBoxMegrendeloNev.Text = string.Empty;
        }

        private void KattintaskorGombok()
        {
            ujAdat = false;
            buttonUjMentesf.Visible = false;
            buttonMegrendeloMegse.Visible = false;
            panelModositTorolGombokfutar.Visible = true;
            errorProviderMegrendeloCim.Clear();
            errorProviderMegrendeloNev.Clear();
        }
        private void frissitMegrendelőkDGV()
        {
            //Adattáblát feltölti a repoba lévő pizza listából
            megrendeloDT = repoo.MegrendelokListabolDataTable();
            //Pizza DataGridView-nak a forrása a pizza adattábla
            dataGridViewMegrendelok.DataSource = null;
            dataGridViewMegrendelok.DataSource = megrendeloDT;
        }
        private void dataGridViewMegrendelok_SelectionChanged(object sender, EventArgs e)
        {
            if (ujAdat)
            {
                KattintaskorGombok();
            }
            if (dataGridViewMegrendelok.SelectedRows.Count == 1)
            {
                panelMegrendeloAdatok.Visible = true;
                panelModositTorolMegrendeloAdatok.Visible = true;
                buttonUjMegrendelo.Visible = true;
                textBoxMegrendeloAZ.Text =
                    dataGridViewMegrendelok.SelectedRows[0].Cells[0].Value.ToString();
                textBoxMegrendeloNev.Text =
                    dataGridViewMegrendelok.SelectedRows[0].Cells[1].Value.ToString();
                textBoxMegrendeloCim.Text =
                    dataGridViewMegrendelok.SelectedRows[0].Cells[2].Value.ToString();
            }
            else
            {
                panelMegrendeloAdatok.Visible = false;
                panelModositTorolMegrendeloAdatok.Visible = false;
                buttonUjMegrendelo.Visible = false;
            }

        }

        private void beallitMegrendeloDGV()
        {
            megrendeloDT.Columns[0].ColumnName = "Azonosító";
            megrendeloDT.Columns[0].Caption = "Megrendelő azonosító";
            megrendeloDT.Columns[1].ColumnName = "Név";
            megrendeloDT.Columns[1].Caption = "Megrendelő név";
            megrendeloDT.Columns[2].ColumnName = "Cím";
            megrendeloDT.Columns[2].Caption = "Megrendelő Cím";

            dataGridViewMegrendelok.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dataGridViewMegrendelok.ReadOnly = true;
            dataGridViewMegrendelok.AllowUserToDeleteRows = false;
            dataGridViewMegrendelok.AllowUserToAddRows = false;
            dataGridViewMegrendelok.MultiSelect = false;
        }
        private void buttonBeolvasMegrendelok_Click(object sender, EventArgs e)
        {
            //Adatbázisban pizza tábla kezelése
            RepositoryVevoTableDatabase rtp = new RepositoryVevoTableDatabase();
            //A repo-ba lévő pizza listát feltölti az adatbázisból
            repoo.setMegrendelok(rtp.getVevoFromDatabasePvevoTable());
            frissitMegrendelőkDGV();
            beallitMegrendeloDGV();
            MegrendeloGombokIndulaskor();
            dataGridViewMegrendelok.SelectionChanged += dataGridViewMegrendelok_SelectionChanged;

        }
        private void buttonTorolMegrendelo_Click(object sender, EventArgs e)
        {
            torolHibauzenetet();
            if ((dataGridViewMegrendelok.Rows == null) ||
                (dataGridViewMegrendelok.Rows.Count == 0))
                return;
            //A felhasználó által kiválasztott sor a DataGridView-ban            
            int sor = dataGridViewMegrendelok.SelectedRows[0].Index;
            if (MessageBox.Show(
                "Valóban törölni akarja a sort?",
                "Törlés",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                //1. törölni kell a listából
                int id = -1;
                if (!int.TryParse(
                         dataGridViewMegrendelok.SelectedRows[0].Cells[0].Value.ToString(),
                         out id))
                    return;
                try
                {
                    repoo.MegrendeloTorlesListabolIDAlapjan(id);
                }
                catch (RepositoryExceptionCantDelete recd)
                {
                    kiirHibauzenetet(recd.Message);
                    Debug.WriteLine("A Megrendelő törlés nem sikerült, nincs a listába!");
                }
                //2. törölni kell az adatbázisból
                RepositoryVevoTableDatabase rdtp = new RepositoryVevoTableDatabase();
                try
                {
                    rdtp.deleteVevoFromDatabase(id);
                }
                catch (Exception ex)
                {
                    kiirHibauzenetet(ex.Message);
                }
                //3. frissíteni kell a DataGridView-t  
                frissitMegrendelőkDGV();
                if (dataGridViewMegrendelok.SelectedRows.Count <= 0)
                {
                    buttonUjMegrendelo.Visible = true;
                }
                beallitMegrendeloDGV();
            }
        }

        private void buttonMegrendeloModosit_Click(object sender, EventArgs e)
        {
            torolHibauzenetet();
            errorProviderMegrendeloNev.Clear();
            errorProviderMegrendeloCim.Clear();
            try
            {
                Megrendelo modosult = new Megrendelo(
                    Convert.ToInt32(textBoxMegrendeloAZ.Text),
                    textBoxMegrendeloNev.Text,
                    textBoxMegrendeloCim.Text
                    );
                int azonosito = Convert.ToInt32(textBoxMegrendeloAZ.Text);
                //1. módosítani a listába
                try
                {
                    repoo.FrissitMegrendelotListaban(azonosito, modosult);
                }
                catch (Exception ex)
                {
                    kiirHibauzenetet(ex.Message);
                    return;
                }
                //2. módosítani az adatbáziba
                RepositoryVevoTableDatabase rdtp = new RepositoryVevoTableDatabase();
                try
                {
                    rdtp.updateVevoInDatabase(azonosito, modosult);
                }
                catch (Exception ex)
                {
                    kiirHibauzenetet(ex.Message);
                }
                //3. módosítani a DataGridView-ban           
                frissitMegrendelőkDGV();
            }
            catch (MegrendeloNevValidation mnv)
            {
                errorProviderMegrendeloNev.SetError(textBoxMegrendeloNev, "Hiba a névben!");
            }
            catch (MegrendeloCimValidation mcv)
            {
                errorProviderMegrendeloCim.SetError(textBoxMegrendeloCim, "Hiba a címben!");
            }
            catch (RepositoryExceptionCantModified recm)
            {
                kiirHibauzenetet(recm.Message);
                Debug.WriteLine("Módosítás nem sikerült, a megrendelő nincs a listába!");
            }
            catch (Exception ex)
            { }
        }
        private void buttonMegrendeloMentes_Click(object sender, EventArgs e)
        {
            torolHibauzenetet();
            errorProviderMegrendeloCim.Clear();
            errorProviderMegrendeloNev.Clear();
            try
            {
                Megrendelo ujM = new Megrendelo(
                    Convert.ToInt32(textBoxMegrendeloAZ.Text),
                    textBoxMegrendeloNev.Text,
                    textBoxMegrendeloCim.Text
                    );
                int azonosito = Convert.ToInt32(textBoxMegrendeloAZ.Text);
                //1. Hozzáadni a listához
                try
                {

                    repoo.HozzaAdMegrendeloListahoz(ujM);

                }
                catch (Exception ex)
                {
                    kiirHibauzenetet(ex.Message);
                    return;
                }
                //2. Hozzáadni az adatbázishoz
                RepositoryVevoTableDatabase rdtp = new RepositoryVevoTableDatabase();
                try
                {

                    rdtp.insertVevoToDatabase(ujM);

                }
                catch (Exception ex)
                {
                    kiirHibauzenetet(ex.Message);
                }
                //3. Frissíteni a DataGridView-t
                frissitMegrendelőkDGV();
                beallitGombokatUjMegrendeloMegsemEsMentes();
                if (dataGridViewMegrendelok.SelectedRows.Count == 1)
                {
                    beallitMegrendeloDGV();
                }

            }
            catch (MegrendeloNevValidation mnv)
            {
                errorProviderMegrendeloNev.SetError(textBoxMegrendeloNev, "Hiba a névben!");
            }
            catch (MegrendeloCimValidation mcv)
            {
                errorProviderMegrendeloCim.SetError(textBoxMegrendeloCim, "Hiba a címben!");
            }
            catch (Exception ex)
            {
            }
        }
        private void buttonUjMegrendelo_Click(object sender, EventArgs e)
        {
            ujAdat = true;
            beallitGombokatTextboxokatUjMegrendelonel();
            int ujVevoAz = repoo.getNextMegrendeloID();
            textBoxMegrendeloAZ.Text = ujVevoAz.ToString();
        }

        private void buttonMegrendeloMegse_Click(object sender, EventArgs e)
        {
            beallitGombokatUjMegrendeloMegsemEsMentes();
        }
        private void textBoxMegrendeloNev_TextChanged(object sender, EventArgs e)
        {
            ujMegsemGombokKezelese();
        }

        private void textBoxMegrendeloCim_TextChanged(object sender, EventArgs e)
        {
            ujMegsemGombokKezelese();
        }
    }
}
