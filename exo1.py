import sqlite3
from flask import Flask, request

app = Flask(__name__)

users = []
orders = {}
current_order = []
db = sqlite3.connect("data.db", check_same_thread=False)
cursor = db.cursor()

@app.route("/register", methods=["POST"])
def register():
    data = request.form
    u = {
        "name": data["name"],
        "email": data["email"],
        "age": data["age"],
        "role": "customer" if int(data["age"]) > 17 else "child"
    }
    users.append(u)
    cursor.execute("INSERT INTO log (action, user) VALUES (?, ?)", ("register", data["email"]))
    db.commit()
    return "OK"

@app.route("/order/add", methods=["POST"])
def add_to_order():
    p = request.form["product"]
    q = int(request.form["q"])
    r = cursor.execute("SELECT name, price, stock FROM products WHERE name='" + p + "'").fetchone()
    if r[2] < q:
        return "STOCK"
    current_order.append({"p": r[0], "q": q, "t": q * r[1]})
    cursor.execute("UPDATE products SET stock=stock-" + str(q) + " WHERE name='" + p + "'")
    db.commit()
    return "ADDED"

@app.route("/order/confirm")
def confirm():
    total = 0
    for i in current_order:
        total += i["t"]
    email = request.args.get("email")
    if email not in orders:
        orders[email] = []
    orders[email].append(current_order.copy())
    cursor.execute("INSERT INTO log (action, user) VALUES (?, ?)", ("order", email))
    db.commit()
    html = "<h1>Order confirmed</h1>"
    for i in current_order:
        html += f"<div>{i['p']} - {i['q']} - {i['t']}</div>"
    html += "<p>Total: " + str(total) + "</p>"
    current_order.clear()
    return html

@app.route("/admin/users")
def admin_users():
    if request.args.get("key") == "1234":
        out = "<h2>Users</h2><ul>"
        for u in users:
            out += f"<li>{u['name']} - {u['email']} - {u['role']}</li>"
        return out + "</ul>"
    return "NO"

@app.route("/stats")
def stats():
    logs = cursor.execute("SELECT action, user FROM log").fetchall()
    txt = ""
    for l in logs:
        txt += f"{l[0]} - {l[1]}<br>"
    p = cursor.execute("SELECT name, stock FROM products").fetchall()
    for x in p:
        txt += f"{x[0]} : {x[1]}<br>"
    return txt

if __name__ == "__main__":
    app.run()
