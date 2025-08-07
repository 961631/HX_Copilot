package com.secom.emergency.repository;

import com.secom.emergency.entity.NewTerminalRegist;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;

@Repository
public interface NewTerminalRegistRepository extends JpaRepository<NewTerminalRegist, Long> {
    
    /**
     * 端末製造番号で検索
     */
    Optional<NewTerminalRegist> findByTerminalSerialNumber(String terminalSerialNumber);
    
    /**
     * 端末製造番号の存在確認
     */
    boolean existsByTerminalSerialNumber(String terminalSerialNumber);
}