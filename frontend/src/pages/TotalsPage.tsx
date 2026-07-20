import { useEffect, useState } from "react";
import { PersonTotalsList } from "../components/PersonTotalsList";
import { SummaryCard } from "../components/SummaryCard";
import { totalsService } from "../services/totalsService";
import type { TotalsResponse } from "../types/totals";
import { getErrorMessage } from "../utils/getErrorMessage";
import "./totals.css";

const currencyFormatter = new Intl.NumberFormat("pt-BR", {
  style: "currency",
  currency: "BRL",
});

const emptyTotals: TotalsResponse = {
  persons: [],
  general: {
    totalIncome: 0,
    totalExpense: 0,
    balance: 0,
  },
};

export function TotalsPage() {
  const [totals, setTotals] =
    useState<TotalsResponse>(emptyTotals);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");

  async function loadTotals() {
    try {
      setIsLoading(true);
      setError("");

      const data = await totalsService.get();

      setTotals(data);
    } catch (requestError) {
      setError(
        getErrorMessage(
          requestError,
          "Não foi possível carregar os totais.",
        ),
      );
    } finally {
      setIsLoading(false);
    }
  }

  useEffect(() => {
    let isCancelled = false;

    totalsService
      .get()
      .then((data) => {
        if (!isCancelled) {
          setTotals(data);
        }
      })
      .catch((requestError: unknown) => {
        if (!isCancelled) {
          setError(
            getErrorMessage(
              requestError,
              "Não foi possível carregar os totais.",
            ),
          );
        }
      })
      .finally(() => {
        if (!isCancelled) {
          setIsLoading(false);
        }
      });

    return () => {
      isCancelled = true;
    };
  }, []);

  return (
    <div className="page">
      <header className="page-header">
        <div>
          <span className="eyebrow">Controle residencial</span>
          <h1>Totais</h1>
          <p>
            Consulte receitas, despesas e saldos individuais e
            gerais.
          </p>
        </div>

        <button
          className="secondary-button"
          type="button"
          disabled={isLoading}
          onClick={() => void loadTotals()}
        >
          {isLoading ? "Atualizando..." : "Atualizar dados"}
        </button>
      </header>

      {error && (
        <div className="feedback feedback-error" role="alert">
          {error}
        </div>
      )}

      {isLoading ? (
        <section className="card loading-card">
          <p>Carregando totais...</p>
        </section>
      ) : (
        <>
          <section className="summary-grid">
            <SummaryCard
              label="Receitas gerais"
              value={currencyFormatter.format(
                totals.general.totalIncome,
              )}
              variant="income"
            />

            <SummaryCard
              label="Despesas gerais"
              value={currencyFormatter.format(
                totals.general.totalExpense,
              )}
              variant="expense"
            />

            <SummaryCard
              label="Saldo líquido"
              value={currencyFormatter.format(
                totals.general.balance,
              )}
            />
          </section>

          <PersonTotalsList totals={totals.persons} />
        </>
      )}
    </div>
  );
}