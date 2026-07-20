import { useMemo, useState, type FormEvent } from "react";
import type { Person } from "../types/person";
import {
  TransactionType,
  type CreateTransaction,
} from "../types/transaction";

interface TransactionFormProps {
  persons: Person[];
  isSubmitting: boolean;
  onSubmit: (
    transaction: CreateTransaction,
  ) => Promise<boolean>;
}

export function TransactionForm({
  persons,
  isSubmitting,
  onSubmit,
}: TransactionFormProps) {
  const [description, setDescription] = useState("");
  const [amount, setAmount] = useState("");
  const [type, setType] = useState<TransactionType>(
    TransactionType.Expense,
  );
  const [personId, setPersonId] = useState("");

  const selectedPerson = useMemo(
    () =>
      persons.find(
        (person) => person.id === Number(personId),
      ),
    [personId, persons],
  );

  const selectedPersonIsMinor =
    selectedPerson !== undefined && selectedPerson.age < 18;

  function handlePersonChange(selectedPersonId: string) {
    setPersonId(selectedPersonId);

    const person = persons.find(
      (currentPerson) =>
        currentPerson.id === Number(selectedPersonId),
    );

    // Ao selecionar um menor, o tipo volta para despesa,
    // preservando a regra de negócio também na interface.
    if (person && person.age < 18) {
      setType(TransactionType.Expense);
    }
  }

  async function handleSubmit(
    event: FormEvent<HTMLFormElement>,
  ) {
    event.preventDefault();

    const normalizedDescription = description.trim();
    const parsedAmount = Number(amount);
    const parsedPersonId = Number(personId);

    if (
      !normalizedDescription ||
      parsedAmount <= 0 ||
      parsedPersonId <= 0
    ) {
      return;
    }

    const wasCreated = await onSubmit({
      description: normalizedDescription,
      amount: parsedAmount,
      type,
      personId: parsedPersonId,
    });

    // Os campos são limpos somente após confirmação do backend.
    if (wasCreated) {
      setDescription("");
      setAmount("");
      setType(TransactionType.Expense);
    }
  }

  if (persons.length === 0) {
    return (
      <section className="card">
        <div className="section-heading">
          <div>
            <span className="eyebrow">Nova movimentação</span>
            <h2>Adicionar transação</h2>
          </div>
        </div>

        <div className="empty-state">
          <strong>Cadastre uma pessoa primeiro</strong>
          <p>
            Toda transação precisa estar vinculada a uma pessoa.
          </p>
        </div>
      </section>
    );
  }

  return (
    <section className="card">
      <div className="section-heading">
        <div>
          <span className="eyebrow">Nova movimentação</span>
          <h2>Adicionar transação</h2>
        </div>
      </div>

      <form
        className="transaction-form"
        onSubmit={handleSubmit}
      >
        <label className="form-field">
          <span>Descrição</span>

          <input
            type="text"
            value={description}
            onChange={(event) =>
              setDescription(event.target.value)
            }
            maxLength={200}
            placeholder="Ex.: Conta de energia"
            required
            disabled={isSubmitting}
          />
        </label>

        <label className="form-field">
          <span>Valor</span>

          <input
            type="number"
            value={amount}
            onChange={(event) => setAmount(event.target.value)}
            min="0.01"
            step="0.01"
            placeholder="Ex.: 250,50"
            required
            disabled={isSubmitting}
          />
        </label>

        <label className="form-field">
          <span>Pessoa</span>

          <select
            value={personId}
            onChange={(event) =>
              handlePersonChange(event.target.value)
            }
            required
            disabled={isSubmitting}
          >
            <option value="">Selecione uma pessoa</option>

            {persons.map((person) => (
              <option key={person.id} value={person.id}>
                {person.name} — {person.age} anos
              </option>
            ))}
          </select>
        </label>

        <label className="form-field">
          <span>Tipo</span>

          <select
            value={type}
            onChange={(event) =>
              setType(
                Number(event.target.value) as TransactionType,
              )
            }
            disabled={isSubmitting}
          >
            <option value={TransactionType.Expense}>
              Despesa
            </option>

            <option
              value={TransactionType.Income}
              disabled={selectedPersonIsMinor}
            >
              Receita
            </option>
          </select>
        </label>

        {selectedPersonIsMinor && (
          <p className="form-hint">
            Pessoas menores de 18 anos podem cadastrar apenas
            despesas.
          </p>
        )}

        <button
          className="primary-button"
          type="submit"
          disabled={isSubmitting}
        >
          {isSubmitting
            ? "Cadastrando..."
            : "Cadastrar transação"}
        </button>
      </form>
    </section>
  );
}