﻿using CoffeeFancy.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeFancy.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;

        public static FoodDAO Instance
        {
            get { if (instance == null) instance = new FoodDAO(); return FoodDAO.instance; }
            private set { FoodDAO.instance = value; }
        }

        private FoodDAO() { }

        public List<Food> GetFoodByCategoryID(int id)
        {
            List<Food> list = new List<Food> ();

            string query = "select * from Food where idCategory =" + id;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach(DataRow item in data.Rows) {
                Food food = new Food(item);
                list.Add(food); 
            }

            return list;

        }

        public List<Food> GetListFood()
        {
            List<Food> list = new List<Food>();

            string query = "select * from Food ";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }

            return list;
        }

        public bool InsertFood(string name, int id, float price)
        {
            string query = string.Format("insert dbo.Food (name , idCategory, price) values (N'{0}', {1}, {2})", name, id, price);
            
            int result = DataProvider.Instance.ExecuteNonQuery(query);
        
            return result > 0;
        }
    }
}
