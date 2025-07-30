const mongoose = require("mongoose");

// Define the structure of an Expense document
const ExpenseSchema = new mongoose.Schema({
  name: {
    type: String,
    required: true
  },
  amount: {
    type: Number,
    required: true
  },
  date: {
    type: Date,
    default: Date.now
  }
});

// Export a Mongoose model called "Expense"
module.exports = mongoose.model("Expense", ExpenseSchema);
