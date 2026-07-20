import type { PersonTotals } from "../types/totals";

interface PersonTotalsListProps {
  totals: PersonTotals[];
}

const currencyFormatter = new Intl.NumberFormat("pt-BR", {
  style: "currency",
  currency: "BRL",
});

export function PersonTotalsList({
  totals,
}: PersonTotalsListProps) {
  return (
    <section className="card">
      <div className="section-heading">
        <div>
          <span className="eyebrow">Resumo individual</span>
          <h2>Totais por pessoa</h2>
        </div>

        <span className="counter-badge">{totals.length}</span>
      </div>

      {totals.length === 0 ? (
        <div className="empty-state">
          <strong>Nenhuma pessoa cadastrada</strong>
          <p>
            Cadastre pessoas e transações para visualizar os
            totais.
          </p>
        </div>
      ) : (
        <div className="table-wrapper">
          <table>
            <thead>
              <tr>
                <th>Pessoa</th>
                <th className="value-column">Receitas</th>
                <th className="value-column">Despesas</th>
                <th className="value-column">Saldo</th>
              </tr>
            </thead>

            <tbody>
              {totals.map((personTotals) => {
                const balanceClass =
                  personTotals.balance > 0
                    ? "income-value"
                    : personTotals.balance < 0
                      ? "expense-value"
                      : "";

                return (
                  <tr key={personTotals.personId}>
                    <td>
                      <div className="person-name">
                        <span className="person-avatar">
                          {personTotals.personName
                            .charAt(0)
                            .toUpperCase()}
                        </span>

                        <div>
                          <strong>
                            {personTotals.personName}
                          </strong>
                          <small>
                            ID #{personTotals.personId}
                          </small>
                        </div>
                      </div>
                    </td>

                    <td className="value-column income-value">
                      {currencyFormatter.format(
                        personTotals.totalIncome,
                      )}
                    </td>

                    <td className="value-column expense-value">
                      {currencyFormatter.format(
                        personTotals.totalExpense,
                      )}
                    </td>

                    <td
                      className={`value-column ${balanceClass}`}
                    >
                      {currencyFormatter.format(
                        personTotals.balance,
                      )}
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      )}
    </section>
  );
}