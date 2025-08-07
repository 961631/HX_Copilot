package com.secom.emergency.repository;

import com.secom.emergency.entity.TerminalSimHimoduke;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.time.LocalDateTime;
import java.util.Optional;

@Repository
public interface TerminalSimHimodukeRepository extends JpaRepository<TerminalSimHimoduke, Long> {
    
    /**
     * IMEIとICCIDの組み合わせで検索
     */
    Optional<TerminalSimHimoduke> findByImeiAndIccid(String imei, String iccid);
    
    /**
     * IMEIとICCIDの組み合わせの存在確認
     */
    boolean existsByImeiAndIccid(String imei, String iccid);
    
    /**
     * 当日登録分を取得（ページング対応）
     */
    @Query("SELECT t FROM TerminalSimHimoduke t WHERE t.createdAt >= :startOfDay AND t.createdAt < :endOfDay ORDER BY t.createdAt DESC")
    Page<TerminalSimHimoduke> findTodayRegistrations(@Param("startOfDay") LocalDateTime startOfDay, 
                                                    @Param("endOfDay") LocalDateTime endOfDay, 
                                                    Pageable pageable);
    
    /**
     * 当日登録分の件数を取得
     */
    @Query("SELECT COUNT(t) FROM TerminalSimHimoduke t WHERE t.createdAt >= :startOfDay AND t.createdAt < :endOfDay")
    long countTodayRegistrations(@Param("startOfDay") LocalDateTime startOfDay, 
                                @Param("endOfDay") LocalDateTime endOfDay);
}