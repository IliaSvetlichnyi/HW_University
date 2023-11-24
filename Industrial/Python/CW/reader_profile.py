from tkinter import ttk
import tkinter as tk
import pandas as pd

def reader_profile(data, document_id):
    # Filter data for the specific document_id
    data_for_document = data[data['env_doc_id'] == document_id].copy()
    # Top ten readers by time spent on the site
    return data_for_document[data_for_document["env_type"] == "reader"].groupby("visitor_uuid").agg({"event_readtime": "sum"}).sort_values(
        by="event_readtime", ascending=False).head(10)


# How this table should be displayed using tkinter

# def display_table(dataframe):
#     # Create a new Tkinter window
#     window = tk.Tk()
#     window.title("Reader Profiles")

#     # Create a Treeview widget
#     tree = ttk.Treeview(window)

#     # Define the column headers
#     tree["columns"] = list(dataframe.columns)
#     tree["show"] = "headings"  # Hide the first empty column

#     # Create the column headings
#     for col in tree["columns"]:
#         tree.heading(col, text=col)

#     # Add the data to the Treeview widget
#     for index, row in dataframe.iterrows():
#         tree.insert("", "end", values=list(row))

#     # Pack the Treeview widget into the window
#     tree.pack(expand=True, fill="both")

#     # Run the Tkinter event loop
#     window.mainloop()


# # Example usage with the dataframe 'top_readers' you got from the 'reader_profile' function
# display_table(top_readers)
