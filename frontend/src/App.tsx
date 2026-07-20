import { useState } from "react";
import { PersonsPage } from "./pages/PersonsPage";
import { TotalsPage } from "./pages/TotalsPage";
import { TransactionsPage } from "./pages/TransactionsPage";

type Page = "persons" | "transactions" | "totals";

function App() {
  const [currentPage, setCurrentPage] =
    useState<Page>("persons");

  return (
    <main className="app-shell">
      <aside className="sidebar">
        <div className="brand">
          <span className="brand-icon">$</span>

          <div>
            <strong>Expense Control</strong>
            <small>Gestão residencial</small>
          </div>
        </div>

        <nav
          className="navigation"
          aria-label="Navegação principal"
        >
          <button
            className={
              currentPage === "persons"
                ? "navigation-item active"
                : "navigation-item"
            }
            type="button"
            onClick={() => setCurrentPage("persons")}
          >
            Pessoas
          </button>

          <button
            className={
              currentPage === "transactions"
                ? "navigation-item active"
                : "navigation-item"
            }
            type="button"
            onClick={() => setCurrentPage("transactions")}
          >
            Transações
          </button>

          <button
            className={
              currentPage === "totals"
                ? "navigation-item active"
                : "navigation-item"
            }
            type="button"
            onClick={() => setCurrentPage("totals")}
          >
            Totais
          </button>
        </nav>
      </aside>

      <div className="main-content">
        {currentPage === "persons" && <PersonsPage />}

        {currentPage === "transactions" && (
          <TransactionsPage />
        )}

        {currentPage === "totals" && <TotalsPage />}
      </div>
    </main>
  );
}

export default App;