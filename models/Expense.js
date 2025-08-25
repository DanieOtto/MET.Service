const mongoose = require("mongoose");
const AutoIncrement = require("mongoose-sequence")(mongoose);

// Define the structure of an Expense document
const ExpenseSchema = new mongoose.Schema({
  id: {
    type: Number,
    unique: true
  },
  name: {
    type: String,
    required: true
  },
  amount: {
    type: Number,
    required: true
  },
  category: {
    type: String,
    required: true
  },
  date: {
    type: Date,
    default: Date.now
  }
});

// Auto-increment `id` field
ExpenseSchema.plugin(AutoIncrement, { inc_field: "id" });

// Export a Mongoose model called "Expense"
module.exports = mongoose.model("Expense", ExpenseSchema);
