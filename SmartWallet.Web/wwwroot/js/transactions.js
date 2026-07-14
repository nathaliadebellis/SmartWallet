document.addEventListener("DOMContentLoaded", () => {

    const transactionTypeSelect = document.getElementById("Type");
    const categorySelect = document.getElementById("CategoryId");

    if (!transactionTypeSelect || !categorySelect) {
        return;
    }

    const categoryEndpoint = "/Categories/GetByTransactionType";

    transactionTypeSelect.addEventListener("change", () => {
        loadCategories();
    });

    initialize();

    async function initialize() {

        if (transactionTypeSelect.value) {
            await loadCategories(categorySelect.dataset.selected);
        }
        else {
            resetCategorySelect("Selecione o tipo primeiro...");
        }

    }

    async function loadCategories(selectedCategoryId = null) {

        resetCategorySelect("Carregando...");

        try {

            const response = await fetch(
                `${categoryEndpoint}?transactionType=${transactionTypeSelect.value}`);

            if (!response.ok) {
                throw new Error("Erro ao carregar categorias.");
            }

            const categories = await response.json();

            categorySelect.innerHTML = "";

            categorySelect.appendChild(
                new Option("Selecione uma categoria...", "")
            );

            categories.forEach(category => {

                const option = new Option(
                    category.name,
                    category.id
                );

                if (selectedCategoryId &&
                    category.id.toString() === selectedCategoryId.toString()) {

                    option.selected = true;

                }

                categorySelect.appendChild(option);

            });

            categorySelect.disabled = false;

        }
        catch (error) {

            console.error(error);

            resetCategorySelect("Erro ao carregar categorias");

        }

    }

    function resetCategorySelect(message) {

        categorySelect.innerHTML = "";

        categorySelect.appendChild(
            new Option(message, "")
        );

        categorySelect.disabled = true;

    }

});