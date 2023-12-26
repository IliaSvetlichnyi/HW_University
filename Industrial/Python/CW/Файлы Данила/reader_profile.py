import tkinter as tk
from tkinter import ttk

import tkinter as tk
import pandas as pd
import json


def show_reader_profile(data, document_id):
    root = tk.Tk()
    root.title("Top Readers by Time Spent")

    data_for_document = data[data['env_doc_id'] == document_id].copy()
    top_readers = data_for_document[data_for_document["env_type"] == "reader"].groupby("visitor_uuid").agg({"event_readtime": "sum"}).sort_values(
        by="event_readtime", ascending=False).head(10)

    tree = ttk.Treeview(root)
    tree["columns"] = ("event_readtime", "visitor_uuid")
    tree.column("#0", width=0, stretch=tk.NO)
    tree.column("event_readtime", anchor=tk.CENTER, width=100)
    tree.column("visitor_uuid", anchor=tk.CENTER, width=150)

    tree.heading("#0", text="", anchor=tk.CENTER)
    tree.heading("event_readtime", text="Event Readtime", anchor=tk.CENTER)
    tree.heading("visitor_uuid", text="Visitor UUID", anchor=tk.CENTER)

    for index, row in top_readers.iterrows():
        tree.insert("", tk.END, text=index, values=(row["event_readtime"], index))

    tree.pack(expand=tk.YES, fill=tk.BOTH)
    root.mainloop()

