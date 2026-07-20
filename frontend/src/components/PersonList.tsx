import type { Person } from "../types/person";

interface PersonListProps {
  persons: Person[];
  deletingPersonId: number | null;
  editingPersonId: number | null;
  onEdit: (person: Person) => void;
  onDelete: (person: Person) => Promise<void>;
}

export function PersonList({
  persons,
  deletingPersonId,
  editingPersonId,
  onEdit,
  onDelete,
}: PersonListProps) {
  return (
    <section className="card">
      <div className="section-heading">
        <div>
          <span className="eyebrow">Moradores</span>
          <h2>Pessoas cadastradas</h2>
        </div>

        <span className="counter-badge">{persons.length}</span>
      </div>

      {persons.length === 0 ? (
        <div className="empty-state">
          <strong>Nenhuma pessoa cadastrada</strong>
          <p>Use o formulário para adicionar o primeiro cadastro.</p>
        </div>
      ) : (
        <div className="table-wrapper">
          <table>
            <thead>
              <tr>
                <th>Nome</th>
                <th>Idade</th>
                <th className="actions-column">Ações</th>
              </tr>
            </thead>

            <tbody>
              {persons.map((person) => {
                const isDeleting =
                  deletingPersonId === person.id;

                const isEditing =
                  editingPersonId === person.id;

                return (
                  <tr key={person.id}>
                    <td>
                      <div className="person-name">
                        <span className="person-avatar">
                          {person.name.charAt(0).toUpperCase()}
                        </span>

                        <div>
                          <strong>{person.name}</strong>
                          <small>ID #{person.id}</small>
                        </div>
                      </div>
                    </td>

                    <td>{person.age} anos</td>

                    <td className="actions-column">
                      <div className="table-actions">
                        <button
                          className="edit-button"
                          type="button"
                          disabled={isDeleting || isEditing}
                          onClick={() => onEdit(person)}
                        >
                          {isEditing ? "Editando" : "Editar"}
                        </button>

                        <button
                          className="danger-button"
                          type="button"
                          disabled={isDeleting}
                          onClick={() => void onDelete(person)}
                        >
                          {isDeleting ? "Excluindo..." : "Excluir"}
                        </button>
                      </div>
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