using CSharpEgitimKampi501.Dtos;
using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpEgitimKampi501
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        StringBuilder l_sb = new StringBuilder();
        SqlConnection connection = new SqlConnection("Server=.;Database=EgitimKampi501Db;Trusted_Connection=True;");
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            l_sb.Clear();
            l_sb.AppendLine("select COUNT(*) from TblProduct");
            var totalBookCount = await connection.QueryFirstOrDefaultAsync<int>(l_sb.ToString());
            lblTotalBookCount.Text = totalBookCount.ToString();

            l_sb.Clear();
            l_sb.AppendLine("select ISNULL(P.ProductName,'') from TblProduct P with(nolock) where P.ProductPrice = (select MAX(PP.ProductPrice) from TblProduct PP with(nolock))");
            var maxBookPrice = await connection.QueryFirstOrDefaultAsync<string>(l_sb.ToString());
            lblMaxPriceBookName.Text = maxBookPrice.ToString();

            l_sb.Clear();
            l_sb.AppendLine("select COUNT(distinct(ProductCategory)) categoryCount  from TblProduct ");
            var totalCategoryCount = await connection.QueryFirstOrDefaultAsync<int>(l_sb.ToString());
            lblCategoryCount.Text = totalCategoryCount.ToString();
        }

        private async void btnList_Click(object sender, EventArgs e)
        {
            l_sb.Clear();
            l_sb.AppendLine("select * from TblProduct");
            var values = await connection.QueryAsync<ResultProductDto>(l_sb.ToString());
            dataGridView1.DataSource = values;
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            CreateProductDto createProductDto = new CreateProductDto
            {
                ProductName = txtProductName.Text,
                ProductCategory = txtCategory.Text,
                ProductPrice = decimal.Parse(txtProductPrice.Text),
                ProductStock = int.Parse(txtProductStock.Text)
            };
            l_sb.Clear();
            l_sb.AppendLine("insert into TblProduct (ProductName,ProductStock,ProductPrice,ProductCategory) values (@productName,@productStock,@productPrice,@productCategory)");

            var parameters = new DynamicParameters();
            parameters.Add("@productName", createProductDto.ProductName);
            parameters.Add("@productStock", createProductDto.ProductStock);
            parameters.Add("@productPrice", createProductDto.ProductPrice);
            parameters.Add("@productCategory", createProductDto.ProductCategory);

            await connection.ExecuteAsync(l_sb.ToString(), parameters);
            MessageBox.Show("Yeni Kitap Ekleme İşlemi Başarılı");


        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtProductId.Text);
            l_sb.Clear();
            l_sb.AppendLine("delete from TblProduct where ProductId = @productId");
            var parameters = new DynamicParameters();
            parameters.Add("@productId", id);
            await connection.ExecuteAsync(l_sb.ToString(), parameters);
            MessageBox.Show("Kitap Silme İşlemi Başarılı");
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateProductDto updateProductDto = new UpdateProductDto
            {
                ProductId = int.Parse(txtProductId.Text),
                ProductCategory = txtCategory.Text,
                ProductName = txtProductName.Text,
                ProductPrice = Decimal.Parse(txtProductPrice.Text),
                ProductStock = int.Parse(txtProductStock.Text),
            };
            l_sb.Clear();
            l_sb.AppendLine("update TblProduct set ProductName = @productName, ProductPrice = @productPrice, ProductCategory = @productCategory, ProductStock = @productStock where ProductId = @productId");
            var parameters = new DynamicParameters();
            parameters.Add("@productName", updateProductDto.ProductName);
            parameters.Add("@productPrice", updateProductDto.ProductPrice);
            parameters.Add("@productCategory", updateProductDto.ProductCategory);
            parameters.Add("@productStock", updateProductDto.ProductStock);
            parameters.Add("@productId", updateProductDto.ProductId);

            await connection.ExecuteAsync(l_sb.ToString(), parameters);
            MessageBox.Show(string.Format("Güncelleme Başarılı.\n Yeni Adı : {0}", updateProductDto.ProductName));
        }
    }
}
